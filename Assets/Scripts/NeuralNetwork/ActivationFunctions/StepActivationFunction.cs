using System;

public class StepActivationFunction : IActivationFunction
{
    private double _treshold;

    public StepActivationFunction(double treshold)
    {
        _treshold = treshold;
    }

    public double CalculateOutput(double input)
    {
        //Debug.Log("Output bez f: " + input);
        return Convert.ToDouble(input > _treshold);
    }
}