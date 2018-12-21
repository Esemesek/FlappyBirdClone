/// <summary>
/// Factory used to create layers.
/// </summary>
public class NeuralLayerFactory
{

    public NeuralLayer CreateNeuralLayer(int numberOfNeurons, double[] weights, IActivationFunction activationFunction, IInputFunction inputFunction)
    {
        var layer = new NeuralLayer(weights);

        for (int i = 0; i < numberOfNeurons; i++)
        {
            var neuron = new Neuron(activationFunction, inputFunction);
            layer.Neurons.Add(neuron);
        }

        return layer;
    }
}