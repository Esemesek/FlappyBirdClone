using System.Collections.Generic;
using System.Linq;

public class WeightedSumFunction : IInputFunction
{
    public double CalculateInput(List<ISynapse> inputs)
    {
        return inputs.Select(x => x.Weight * x.GetOutput()).Sum();
    }
}