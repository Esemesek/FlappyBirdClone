using System;
using UnityEngine;

public class InvertedStepActivationFunction : IActivationFunction
{
    private double _treshold;

    public InvertedStepActivationFunction(double treshold)
    {
        _treshold = treshold;
    }

    public double CalculateOutput(double input)
    {
        return Convert.ToDouble(input < _treshold);
    }
}