using System;
using System.Numerics;

public class Node
{
    private double bias;
    private double[] weights;
    private double recentActivation;

    public Node(int inputSize)
    {
        bias = 0.0; // too lazy to make it anything else
        weights = new double[inputSize];
        for (int i = 0; i < inputSize; i++)
        {
            // I just love stack overflow, hope it's not inefficient
            Random rand = new Random(); //reuse this if you are generating many
            double u1 = 1.0-rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0-rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                        Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double mean = 0.0;
            double stdDev = 1.0;
            double randNormal =
                        mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)

            weights[i] = randNormal;
        }
        recentActivation = 0.0;
    }

    private double sigmoid(double x)
    {
        return 1 / (1 + Math.Exp(-1 * x));
    }

    public double activate(double[] input)
    {
        double weightedSum = 0;
        // I love being inefficient
        // HEY FUTURE ME, IF THE PROGRAM IS SLOW 
        // IT'S BECAUSE YOU REFUSED TO USE ANY EXTERNAL LIBRARIES
        for (int i = 0; i < input.Length; i++)
        {
            weightedSum += input[i] * weights[i];
        }
        recentActivation = sigmoid(weightedSum);
        return recentActivation;
    }

     public double getRecentActivations()
     {
        return 0.0d;
     }
}