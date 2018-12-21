using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeuralLayer
{
    public List<INeuron> Neurons;

    private double[] _weights;

    public NeuralLayer(double[] weights)
    {
        Neurons = new List<INeuron>();
        _weights = weights;
    }

    /// <summary>
    /// Connecting two layers.
    /// </summary>
    public void ConnectLayers(NeuralLayer inputLayer)
    {
        var combos = Neurons.SelectMany(neuron => inputLayer.Neurons,
        (neuron, input) => new { neuron, input });

        var combosList = combos.ToList();

        for (int i = 0; i < combos.Count(); i++)
        {
            combosList[i].neuron.AddInputNeuron(combosList[i].input, _weights[i]);
        }

        //combos.ToList().ForEach(x => x.neuron.AddInputNeuron(x.input));
    }
}