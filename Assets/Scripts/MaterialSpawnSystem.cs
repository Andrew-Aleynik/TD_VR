using System.Collections.Generic;
using UnityEngine;

public class MaterialSpawnSystem : SpawnSystem
{
    public List<GameObject> materials;
    public List<float> materialWeights;
    public Transform spawnPoint;

    public void Start() {
        materialWeights = NormalizeWeights(materialWeights);
    }

    public override void Spawn()
    {
        var materialIndex = GetIndex(materialWeights);
        GameObject material = Instantiate(materials[materialIndex], spawnPoint.position, Quaternion.identity);
        GameManager.Instance.materialPool.Add(material);
    }
}
