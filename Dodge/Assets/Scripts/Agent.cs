using UnityEngine;
using System.Collections.Generic;
using NeuralNetwork;
public class Agent {
    private static int NOA = 9;
    private Queue<double[]> replayBuffer;
    private double[] state;
    private float total_reward;
    public Agent() {
        replayBuffer = new Queue<double[]>();
        state = new double[42];
        total_reward = 0.0f;
    }

    public void reset(){
        state = new double[42];
        total_reward = 0.0f;
    }

    public void play_step(NeuralNetwork.Network network, float epsilon) {
        float done_reward = 0.0f;
        double action = 0;
        // With probability epsilon, take random action
        if(Random.Range(0f, 1f) <= epsilon) 
        {
            action = Random.Range(0, NOA);
        }
        else // Choose the highest Q table value
        {
            double[] activation = network.feedForward(state);
            // choose the max value
        }
    }
}