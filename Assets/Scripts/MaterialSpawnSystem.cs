using System.Collections.Generic;
using UnityEngine;

public class MaterialSpawnSystem : SpawnSystem
{
    public List<GameObject> materials;
    public List<float> materialWeights;
    public Transform spawnPoint;

    private List<int> remainingInitialMaterials = new List<int>();
    private bool isInitialPhase = true;

    void Start() 
    {
        materialWeights = NormalizeWeights(materialWeights);
        InitializeInitialPhase();
    }

    private void InitializeInitialPhase()
    {
        remainingInitialMaterials.Clear();
        for (int i = 0; i < materials.Count; i++)
        {
            remainingInitialMaterials.Add(i);
        }
        
        for (int i = 0; i < remainingInitialMaterials.Count; i++)
        {
            int randomIndex = Random.Range(i, remainingInitialMaterials.Count);
            (remainingInitialMaterials[i], remainingInitialMaterials[randomIndex]) = 
                (remainingInitialMaterials[randomIndex], remainingInitialMaterials[i]);
        }
    }

    public override void Spawn()
    {
        int materialIndex;

        if (isInitialPhase && remainingInitialMaterials.Count > 0)
        {
            int randomPos = Random.Range(0, remainingInitialMaterials.Count);
            materialIndex = remainingInitialMaterials[randomPos];
            remainingInitialMaterials.RemoveAt(randomPos);
            
            if (remainingInitialMaterials.Count == 0)
            {
                isInitialPhase = false;
            }
        }
        else
        {
            materialIndex = GetIndex(materialWeights);
        }

        GameObject material = Instantiate(materials[materialIndex], spawnPoint.position, Quaternion.identity);
        GameManager.Instance.materialPool.Add(material);
    }
}