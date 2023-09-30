using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using NeuralNetwork;
using UnityEngine;

public class PersistentObj : MonoBehaviour
{
    
    public PlayerAI playerAI;
    public NeuralNetwork.Network network;

    private static PersistentObj instance;

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
        playerAI = new PlayerAI();

        // TIME TO START TESTING HOW EFFICIENT THIS neural network is
        double[] input = new double[42];
        for (int i = 0; i < 42; i++)
        {
            input[i] = (double)UnityEngine.Random.Range(0.0f, 1.0f);
        }

        NeuralNetwork.Network network = new NeuralNetwork.Network(42, 9, 5, 25); //75 ms just creating
        network.feedForward(input);
        
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

    public void SaveTrainingData(string data)
    {
        string fileName = "training_data.txt"; // Name of the file
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

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
}