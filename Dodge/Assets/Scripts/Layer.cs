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
            recentActivations[i] = nodes[i].getRecentActivation();
        }
        return recentActivations;
    }

    public List<Node> copyNodes(int nodeInputSize) {
        List<Node> temp = new List<Node>();
        for (int i = 0; i < nodes.Count; i++) {
            Node temp_node = new Node(nodeInputSize);
            temp_node.setBias(nodes[i].getBias());
            temp_node.setWeights(nodes[i].getWeights());
            temp_node.setRecentActivation(nodes[i].getRecentActivation());
            temp.Add(temp_node);
        }
        return temp;
    }

    public int getPrevLayerSize() {
        Node target = nodes[0];
        return target.getWeights().Length;
    }

    public int getLayerSize() {
        return nodes.Count;
    }

    public void setNodes(List<Node> new_nodes) {
        nodes = new_nodes;
    }

    public void SGD(double loss, List<double[]> batch, double[] actions, double[] y) {
        for(int i = 0; i < nodes.Count; i++) {
            nodes[i].SGD(loss, batch, actions, y);
        }
    }

    public void optimize() {
        for(int i = 0; i < nodes.Count; i++) {
            nodes[i].optimize();
        }
    }

    public string toString() {
        string res = "";
        for (int i = 0; i < nodes.Count; i++) {
            res += nodes[i].toString();
            res += "\n";
        }
        return res;
    }
}