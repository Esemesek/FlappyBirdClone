using System;
using System.Security.Cryptography;

public class BetterRandom : RandomNumberGenerator
{
    private static RandomNumberGenerator generator;

    public BetterRandom()
    {
        generator = RandomNumberGenerator.Create();
    }

    public override void GetBytes(byte[] data)
    {
        generator.GetBytes(data);
    }

    public override void GetNonZeroBytes(byte[] data)
    {
        generator.GetBytes(data);
    }

    public double NextDouble()
    {
        byte[] b = new byte[4];
        generator.GetBytes(b);
        return (double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
    }

    public double Range(double minValue, double maxValue)
    {
        return (NextDouble() * (maxValue - minValue)) + minValue;
    }

}