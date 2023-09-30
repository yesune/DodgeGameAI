using System.Collections.Generic;


public class Layer
{
    public List<Node> nodes;

    public Layer(int layerSize, int prevLayerSize)
    {
        nodes = new List<Node>();
        for (int i = 0; i < layerSize; i++)
        {
            nodes.Add(new Node(prevLayerSize));
        }
    }

    public double[] activate(double[] input)
    {
        double[] activations = new double[nodes.Count];
        // after learning about map function, i wish i had it so bad now
        for (int i = 0; i < nodes.Count; i++)
        {
            activations[i] = nodes[i].activate(input);
        }
        return activations;
    }

    public double[] getRecentActivations()
    {
        double[] recentActivations = new double[nodes.Count];
        for(int i = 0; i < recentActivations.Length; i++)
        {
            recentActivations[i] = nodes[i].getRecentActivations();
        }
        return recentActivations;
    }
}