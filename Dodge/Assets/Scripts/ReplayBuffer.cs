using System.Collections.Generic;
using System;
using UnityEngine;

public class ReplayBuffer 
{
    private int punishNumber = 5;
    private List<double[]> buffer;
    private int size;


    public ReplayBuffer(int buffer_size) {
        size = buffer_size;
        buffer = new List<double[]>();
    }

    public int getLength() {
        return buffer.Count;
    }

    public void append(double[] experience) {
        buffer.Add(experience);
        if (buffer.Count > size) {
            buffer.RemoveAt(0);
        }
    }

    public List<double[]> sample(int batchSize) {
        List<double[]> result = new List<double[]>();
        List<int> randomIndices = new List<int>();
        System.Random a = new System.Random();
        int r = 0;
        while (randomIndices.Count < batchSize) {
            r = a.Next(0, buffer.Count);
            if (!randomIndices.Contains(r))
                randomIndices.Add(r);
        }
        for (int i = 0; i < batchSize; i++) {
            result.Add(buffer[randomIndices[i]]);
        }
        return result;
    }

    public void punishRecentActions() {
        for(int i = buffer.Count - punishNumber; i < buffer.Count; i++) {
            buffer[i][35] = -(1 / punishNumber) * (i - (buffer.Count - punishNumber) + 1);
        }
    }
}