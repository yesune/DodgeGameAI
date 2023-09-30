using System.Collections.Generic;

namespace NeuralNetwork
{
    public class Network
    {
        private int inputSize;
        private int outputSize;
        private int numHiddenLayers;
        private int hiddenLayerSize;
        public List<Layer> layers;
        public Layer output; // would it be smart to include output in layers? yea but i wont


        public Network(int i, int o, int n, int h)
        {
            inputSize = i;
            outputSize = o;
            numHiddenLayers = n;
            hiddenLayerSize = h;
            initialize();
        }
        
        public void initialize()
        {
            layers = new List<Layer>();
            layers.Add(new Layer(hiddenLayerSize, inputSize));  // first hidden layer
            for (int i = 1; i < numHiddenLayers; i++)
            {
                layers.Add(new Layer(hiddenLayerSize, hiddenLayerSize));
            }
            output = new Layer(outputSize, hiddenLayerSize);  // output layer
        }

        public double[] feedForward(double[] input)
        {
            double[] activation = input;
            for (int i = 0; i < numHiddenLayers; i++)
            {
                activation = layers[i].activate(activation);
            }
            return activation;
        }

        public void Train(double[] input, double[] target)
        {
            // BACK PROPOGATION SURELY THIS WON'T CRASH MY COMPUTER HAHAHAHAHAHHA
            double[] recentActivations = output.getRecentActivations();
            // loss = target - recentActivations
            double[] outputLoss = new double[recentActivations.Length];
            for(int i = 0; i < target.Length; i++)
            {
                outputLoss[i] = target[i] - recentActivations[i];
            }

            // for each hidden layer do the same
            for (int i = 0; i < numHiddenLayers; i++)
            {
                
                // calculate the loss
            }
        }
    }
}