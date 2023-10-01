using UnityEngine;
using System.Collections.Generic;
using NeuralNetwork;

public class Agent {
    private static int NOA = 9;
    private ReplayBuffer replayBuffer;
    private double[] state;
    private double total_reward;
    private PlayerAI playerAI;
    private int counter;
    public Agent(int bufferSize) {
        replayBuffer = new ReplayBuffer(bufferSize);
        state = new double[34];
        total_reward = 0.0f;
        counter = 0;
    }

    public void reset(){
        state = new double[34];
        total_reward = 0.0f;
    }

    public double play_step(NeuralNetwork.Network network, float epsilon) {
        float done_reward = 0.0f;
        double action = 0;

        // With probability epsilon, take random action
        if(Random.Range(0f, 1f) <= epsilon) 
        {
            action = Random.Range(0, NOA);
        }
        else // Choose the highest Q table value
        {
            double max_activation = -10000;
            int max_index = 0;
            network.feedForward(state);
            double[] activation = network.feedForward(state);
            for(int i = 0; i < activation.Length; i++) {
                if (activation[i] > max_activation){
                    max_activation = activation[i];
                    max_index = i;
                }
            }
            action = max_index;
        }
        return action;
    }

    public double post_step(double[] next_state, float reward, double isDone, double action) {
        // isDone 0 for notDone, 1 for done
        double[] new_state = next_state;
        total_reward += reward;
        counter += 1;
        if (counter == 10) {
            counter = 0;
            double[] experience = new double[34 + 1 + 1 + 1 + 34];
            for(int i = 0; i < 34; i++) {
                experience[i] = state[i];
            }
            // i really hope i never have to change these numbers or else its so much pain
            experience[34] = action;
            experience[35] = reward;
            experience[36] = isDone;
            for (int i = 37; i < experience.Length; i++) {
                experience[i] = next_state[i-37];
            }
            replayBuffer.append(experience);
        }
        state = new_state;
        double done_reward = 0.0;
        if (isDone == 1.0f) {
            done_reward = total_reward;
            reset();
        }
        return done_reward;
    }

    public double[] getState() {
        return state;
    }

    public void readState() {
        // take every close bullet
        double[] new_state = new double[34]; // 8 of (Exists, degrees, distance, angle bullet is shot at)
        for (int j = 0; j < Mathf.Min(playerAI.radar.visibleAngles.Count, 8); j++) {
            new_state[j * 4] = 1.0;
            new_state[j * 4 + 1] = playerAI.radar.visibleAngles[j]/ 180.0f; // [-1, 1]
            new_state[j * 4 + 2] = playerAI.radar.visibleDistance[j] / 6.0f; // [0, 1] maximum distance is 6 (for now) 
            new_state[j * 4 + 3] = playerAI.radar.visibleDirection[j] / 180.0f; // [-1, 1]
        }
        new_state[32] = playerAI.radar.transform.position.x / 10.0f; // normalize positions to [-1, 1]
        new_state[33] = playerAI.radar.transform.position.z / 10.0f; 
        state = new_state;
    }

    public void setPlayerAI() {
        playerAI = GameManager.instance.persistent.playerAI;
    }

    public int getBufferLength() {
        return replayBuffer.getLength();
    }

    public List<double[]> sample(int batchSize) {
        return replayBuffer.sample(batchSize);
    }

    public void punishRecentActions() {
        replayBuffer.punishRecentActions();
    }

    public void incrementCount() {
        counter += 1;
    }

    public void resetCounter() {
        counter = 0;
    }

    public int getCounter() {
        return counter;
    }
}