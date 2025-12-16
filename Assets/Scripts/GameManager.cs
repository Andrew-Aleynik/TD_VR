using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MaterialSpawnSystem materialSpawnSystem;
    public EnemySpawnSystem enemySpawnSystem;
    public ClockRotation clockRotation;
    public RecordManager recordManager;
    private float totalTime = 0.0f; 

    public int maxWaves = 5;
    private int currentWave = 1;

}
