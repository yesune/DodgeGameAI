using System.Collections.Generic;
using UnityEngine;

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

        // Trying to implement back propogation
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

        public int getInputSize() {
            return inputSize;
        }
        public int getOutputSize(){
            return outputSize;
        }
        public int getNumHiddenLayers(){
            return numHiddenLayers;
        }
        public int getHiddenLayerSize() {
            return hiddenLayerSize;
        }

        public List<Layer> copyLayers() {
            List<Layer> temp = new List<Layer>();
            for (int i = 0; i < layers.Count; i++) {
                Layer temp_layer = new Layer(layers[i].getLayerSize(), layers[i].getPrevLayerSize());
                temp_layer.setNodes(layers[i].copyNodes(layers[i].getPrevLayerSize()));
                temp.Add(temp_layer);
            }
            
            return temp;
        }

        public Layer copyOutput() {
            Layer temp_layer = new Layer(output.getLayerSize(), output.getPrevLayerSize());
            temp_layer.setNodes(output.copyNodes(output.getPrevLayerSize()));
            return temp_layer;
        }

        public void setLayers(List<Layer> new_layers, Layer new_output) {
            layers.Clear();
            for(int i = 0; i < new_layers.Count; i++) {
                layers.Add(new_layers[i]);
            }
            output = new_output;
        }

        public void SGD(double loss, List<double[]> batch, double[] actions, double[] y) {
            for (int i = 0; i < numHiddenLayers; i++) {
                layers[i].SGD(loss, batch, actions, y);
            }
            output.SGD(loss, batch, actions, y);
        }

        public void optimize() {
            for (int i = 0; i < numHiddenLayers; i++) {
                layers[i].optimize();
            }
            output.optimize();
        }

        public string toString() {
            string res = "";
            for (int i = 0; i < layers.Count; i++) {
                res += layers[i].toString();
                res += "\n";
            }
            return res;
        }
    }
}