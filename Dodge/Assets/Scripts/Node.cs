using System;
using System.Numerics;
using System.Collections.Generic;

public class Node
{
    private float learning_rate = 0.1f;
    private double bias;
    private double[] weights;
    private double recentActivation;

    private double optimizedBias;
    private double[] optimizedWeights;

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
        optimizedWeights = new double[inputSize];
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
        recentActivation = sigmoid(weightedSum + bias);
        return recentActivation;
    }

    public double getBias() {
        return bias;
    }
    public double[] getWeights() {
        double[] temp = new double[weights.Length];
        for (int i = 0; i < weights.Length; i++) {
            temp[i] = weights[i];
        }
        return temp;
    }
    public double getRecentActivation() {
        return recentActivation;
    }

    public void setBias(double b) {
        bias = b;
    }

    public void setWeights(double[] w) {
        weights = w;
    }

    public void setRecentActivation(double r) {
        recentActivation = r;
    }
     
     // maybe calculate the theoretical derivative and then divide it by the delta
    public void SGD(double loss, List<double[]> batch, double[] actions, double[] y) {
        double newLoss = 0.0;
        optimizedBias = bias;
        bias += 0.01;
        for (int i = 0; i < batch.Count; i++) {
            newLoss += Math.Pow((float)(GameManager.instance.persistent.GetNetwork().feedForward(batch[i])[(int)actions[i]] - y[i]), 2.0f);
        }
        newLoss /= batch.Count;
        double slope = (newLoss - loss) / 0.01;
        double temp = optimizedBias;
        if (slope >= 0.000001) {
            optimizedBias -= slope * learning_rate;
        }
        bias = temp;

        for(int i = 0; i < weights.Length; i++) {
            optimizedWeights[i] = weights[i];
        }
        for (int j = 0; j < weights.Length; j++) {
            newLoss = 0.0;
            weights[j] += 0.01;
            for (int i = 0; i < batch.Count; i++) {
                newLoss += Math.Pow((float)(GameManager.instance.persistent.GetNetwork().feedForward(batch[i])[(int)actions[i]] - y[i]), 2.0f);
            }
            newLoss /= batch.Count;
            slope = (newLoss - loss) / 0.01;
            temp = optimizedWeights[j];
            if (slope >= 0.000001) {
                optimizedWeights[j] -= slope * learning_rate;
            }
            weights[j] = temp;
        }
    }

    public void optimize() {
        for(int i = 0; i < weights.Length; i++) {
            weights[i] = optimizedWeights[i];
        }
        bias = optimizedBias;
    }

    public string toString() {
        string res = "";
        for (int i = 0; i < weights.Length; i++) {
            res += weights[i] + " ";
        }
        res += bias;
        return res;
    }
}