using System;
using UnityEngine;

public class DNA<T>
{
    public T[] Genes { get; private set; }
    public float Fitness { get; private set; }
    
    private Func<T> getRandomGene;
    private Func<int, float> fitnessFunction;
    private BetterRandom betterRandom;

    public DNA(int size, Func<T> getRandomGene, Func<int, float> fitnessFunction, bool shouldInitGenes = true)
    {
        Genes = new T[size];
        this.getRandomGene = getRandomGene;
        this.fitnessFunction = fitnessFunction;
        betterRandom = new BetterRandom();

        if (shouldInitGenes)
        {
            for (int i = 0; i < Genes.Length; i++)
            {
                Genes[i] = getRandomGene();
            }
        }
    }

    public float CalculateFitness(int index)
    {
        Fitness = fitnessFunction(index);
        return Fitness;
    }

    public DNA<T> Crossover(DNA<T> otherParent)
    {
        DNA<T> child = new DNA<T>(Genes.Length, getRandomGene, fitnessFunction, shouldInitGenes: false);
        
        for (int i = 0; i < Genes.Length; i++)
        {
            child.Genes[i] = betterRandom.NextDouble() < 0.5 ? Genes[i] : otherParent.Genes[i];
        }

        return child;
    }

    public void Mutate(float mutationRate)
    {
        for (int i = 0; i < Genes.Length; i++)
        {
            if (betterRandom.NextDouble() < mutationRate)
            {
                Genes[i] = getRandomGene();
            }
        }
    }
    
}