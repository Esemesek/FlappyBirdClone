using System;

public interface ISynapse
{
    double Weight { get; set; }
    double PreviousWeight { get; set; }
    double GetOutput();

    bool IsFromNeuron(Guid fromNeuronId);
    void UpdateWeight(double learningRate, double delta);
    void UpdateWeight(double weight);
}