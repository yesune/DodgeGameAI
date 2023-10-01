using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using NeuralNetwork;
using UnityEngine;

public class PersistentObj : MonoBehaviour
{
    
    public PlayerAI playerAI;
    private NeuralNetwork.Network network;

    private NeuralNetwork.Network targetNetwork;

    private static PersistentObj instance;

    private int counter;

    void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            // Set the instance to this object
            instance = this;
            // Make this object persistent between scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    void Start()
    {
        counter = 0;
        playerAI = new PlayerAI();

        // TIME TO START TESTING HOW EFFICIENT THIS neural network is
        double[] input = new double[34];
        for (int i = 0; i < 34; i++)
        {
            input[i] = (double)UnityEngine.Random.Range(0.0f, 1.0f);
        }

        network = new NeuralNetwork.Network(34, 9, 5, 25); //75 ms just creating
        network.feedForward(input);
        targetNetwork = copyNetwork(network);
        
        for (int i = 0; i < playerAI.visions.Length; i++) {
             playerAI.visions[i] = GameObject.Find("Vision" + i).GetComponent<Vision>();
        }

        print("starting");
    }

    public void restart(Vision[] v) {
        playerAI.setVisions(v);
    }

    public void setRadar(Radar radar) {
        playerAI.setRadar(radar);
    }

    public void SaveTrainingData()
    {
        string fileName = "training_data.txt"; // Name of the file
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string data = network.toString();
        try
        {
            // Use File.WriteAllText to overwrite the file with new data
            File.WriteAllText(filePath, data);

            Debug.Log("Training data saved to: " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving training data: " + e.Message);
        }
    }

    public NeuralNetwork.Network GetNetwork() {
        return network;
    }

    public NeuralNetwork.Network getTargetNetwork() {
        return targetNetwork;
    }

    public NeuralNetwork.Network copyNetwork(NeuralNetwork.Network network) {
        NeuralNetwork.Network temp = new NeuralNetwork.Network(network.getInputSize(), network.getOutputSize(),
         network.getNumHiddenLayers(), network.getHiddenLayerSize());
        // we have to copy all the layers
        temp.setLayers(temp.copyLayers(), temp.copyOutput());
        return temp;
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

    public void saveNetwork() {
        targetNetwork = copyNetwork(network);
    }
}