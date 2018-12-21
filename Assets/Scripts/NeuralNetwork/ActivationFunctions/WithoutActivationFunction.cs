using System;
using UnityEngine;

public class WithoutActivationFunction : IActivationFunction
{
    public double CalculateOutput(double input)
    {
        return input;
    }
}