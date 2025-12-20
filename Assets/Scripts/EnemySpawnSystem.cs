using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnSystem : SpawnSystem
{
    public List<Transform> spawnPoints;
    public List<float> spawnPointWeights;
    public List<GameObject> enemies;
    public List<float> enemyWeights;

    public void Start() 
    {
        spawnPointWeights = NormalizeWeights(spawnPointWeights);
        enemyWeights = NormalizeWeights(enemyWeights);
    }

    public override void Spawn() 
    {
        int spawnPointIndex = GetIndex(spawnPointWeights);
        Transform spawnPoint = spawnPoints[spawnPointIndex];
        int enemyIndex = GetIndex(enemyWeights);
        GameObject enemy = enemies[enemyIndex];
        GameObject enemyInstance = Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        GameManager.Instance.enemyPool.Add(enemyInstance);
    }
}
