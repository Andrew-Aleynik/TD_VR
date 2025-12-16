using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnSystem : MonoBehaviour
{
    public abstract void Spawn();

    protected List<float> NormalizeWeights(List<float> weights) 
    {
        List<float> normalizedWeights = new List<float>();

        float sum = 0;
        foreach(float weight in weights)
        {
            sum += weight;
        }

        foreach(float weight in weights)
        {
            normalizedWeights.Add(weight / sum);
        }

        return normalizedWeights;
    }

    protected int GetIndex(List<float> weights)
    {
        float rand = Random.value;
        for (int i = 0; i < weights.Count; i++)
        {
            rand -= weights[i];
            if (rand <= 0)
            {
                return i;
            }
        }
        return weights.Count - 1;
    }
}