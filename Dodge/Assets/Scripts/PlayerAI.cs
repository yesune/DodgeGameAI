using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerAI
{
    // Hyperparameters
    private float alpha = 0.1f; // learning rate
    private float gamma = 0.99f; // discount factor
    private float epsilon = 0.99f; // Exploration rate
    private float epsilon_decay = .999985f;
    private float epsilon_min = 0.02f;
    private int batch_size = 32;
    private int replay_size = 1000;
    private float learning_rate = 0.0001f;
    private int sync_target_frames = 10000;
    private int replay_start_size = 1000;
    public int episodes = 100000;
    public int episode_counter = 0;

    private static int NOA = 9; // 8 directions + standing still

    public Vision[] visions = new Vision[8];
    public Radar radar;

    public float[,] Q = new float[256, NOA];
    private int state;
    private int action;
    public Agent agent;

    // There will be 34 inputs , 40 dedicated for vision
    // 8 of (Bool isPresent, x pos, y pos, x vel, y vel)
    // 2 last ones represent position of player
    //
    // There will also be another double for the action taken
    // There will be a another double for the reward
    // One more for if it's done
    // Another 34 for the new state


    public PlayerAI() {
        state = 0;
        action = 0;
        agent = new Agent(replay_size);
    }

    public int[] getAction() {
        action = 0;
        // With probability epsilon, take random action
        if(Random.Range(0f, 1f) <= epsilon) 
        {
            action = Random.Range(0, NOA);
        }
        else // Choose the highest Q table value
        {
            float maxQ = Mathf.NegativeInfinity;
            for (int i = 0; i < NOA; i++) {
                if (Q[state, i] > maxQ) {
                    action = i;
                    maxQ = Q[state, i];
                }
            }
        }

        // Execute chosen action and observe new state and reward
        int xInput = 0;
        int zInput = 0;
        
        switch(action) 
        {
            case 0:
                break;
            case 1:
                zInput = 1;
                break;
            case 2:
                xInput = 1;
                zInput = 1;
                break;
            case 3:
                xInput = 1;
                break;
            case 4:
                xInput = 1;
                zInput = -1;
                break;
            case 5:
                zInput = -1;
                break;
            case 6:
                xInput = -1;
                zInput = -1;
                break;
            case 7:
                xInput = -1;
                break;
            case 8:
                xInput = -1;
                zInput = 1;
                break;
        }
        int[] res = new int[2];
        res[0] = xInput;
        res[1] = zInput;
        return res;
    }

    public void updateQ() {
        int newState = getState();
        float reward = 0.01f;
        float maxQNewState = float.MinValue;

        // Loop through all possible actions in newState
        for (int i = 0; i < NOA; i++) {
            if (Q[newState, i] > maxQNewState) {
                maxQNewState = Q[newState, i];
            }
        }

        Q[state, action] = Q[state, action] + alpha * (reward + gamma * maxQNewState - Q[state, action]);
        state = newState;
    }

    public int getState()
    {
        int state = 0;
        int factor2 = 1;
        
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < radar.visibleAngles.Count; j++) {
                if (Mathf.Floor(radar.visibleAngles[j]/45) == i) {
                    state += factor2;
                    break;
                }
            }
            factor2 *= 2;
        }
        return state;
    }

    public void onDeath() {
        Q[state, action] = Q[state, action] + alpha * (-1 - Q[state, action]);
    }

    public void setVisions(Vision[] v) {
        visions = v;
    }

    public void setRadar(Radar r) {
        radar = r;
    }

    public float getEpsilon() {
        return epsilon;
    }

    public void decrementEpsilon() {
        epsilon = Mathf.Max(epsilon_decay * epsilon, epsilon_min);
    }

    public int getReplayStartSize() {
        return replay_start_size;
    }

    public float getGamma() {
        return gamma;
    }

    public int getBatchSize() {
        return batch_size;
    }

    public void train() {
        if(agent.getBufferLength() >= replay_start_size) {
            // do some training :)
            Debug.Log("training");
            List<double[]> sample = agent.sample(getBatchSize());
            double[] y = new double[sample.Count];
            double[] next_state;
            for (int i = 0; i < sample.Count; i++) {
                double[] experience = sample[i];
                y[i] = 0.0;
                if (experience[36] == -1) {
                    y[i] = -1.0;
                }
                else {
                    double next_max = (double)Mathf.NegativeInfinity;
                    next_state = new double[34];
                    for (int j = 37; j < experience.Length; j++) {
                        next_state[j-37] = experience[j];
                    }
                    double[] next_output = GameManager.instance.persistent.getTargetNetwork().feedForward(next_state);
                    for (int j = 0; j < next_output.Length; j++) {
                        if (next_max < next_output[j]) {
                            next_max = next_output[j];
                        }
                    }
                    y[i] = 0.001 + gamma * next_max;
                }
            }
            // Time to calculate the loss
            double loss = 0.0;
            for (int i = 0; i < sample.Count; i++) {
                // 34 for action, 2 for square
                double[] experience = sample[i];
                next_state = new double[34];
                    for (int j = 37; j < experience.Length; j++) {
                        next_state[j-37] = experience[j];
                    }
                loss += Mathf.Pow((float)(GameManager.instance.persistent.GetNetwork().feedForward(next_state)[(int)sample[i][34]] - y[i]), 2.0f);
            }
            loss /= sample.Count;
            Debug.Log(loss);
            // thank god for this post reminding me of how to estimate this
            // https://www.alanzucconi.com/2017/04/10/gradient-descent/
            List<double[]> inputs = new List<double[]>();
            double[] actions = new double[sample.Count];
            for(int i = 0; i < sample.Count; i++) {
                double[] input = new double[34];
                for (int j = 0; j < 34; j++) {
                    input[j] = sample[i][j];
                }
                inputs.Add(input);
                actions[i] = sample[i][34];
            }
            GameManager.instance.persistent.GetNetwork().SGD(loss, inputs, actions, y);
            GameManager.instance.persistent.GetNetwork().optimize();
            GameManager.instance.persistent.incrementCount();
            if (GameManager.instance.persistent.getCounter() == 10) { // sync every 10 rounds ig
                GameManager.instance.persistent.saveNetwork();
                GameManager.instance.persistent.resetCounter();
            }
        }
    }
}