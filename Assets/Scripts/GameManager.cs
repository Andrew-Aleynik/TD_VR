using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public MaterialSpawnSystem materialSpawnSystem;
    public EnemySpawnSystem enemySpawnSystem;
    public ClockRotation clockRotation;
    public GameObjectPool enemyPool = new GameObjectPool(20);
    public GameObjectPool materialPool = new GameObjectPool(20);
    public GameObjectPool towerPool = new GameObjectPool(20);
    public GameObjectPool projectilePool = new GameObjectPool(10);
    public SceneLoader sceneLoader;
    private int currentWave;
    public int totalWaves;
    public List<int> enemyNumber;
    public List<int> materialNumber;
    private float time;
    private float totalTime;
    private int score;
    private bool isGameEnd = true;
    public float assembly_time = 30f;
    public float wave_time = 40f; 
    public Button startGameButton;
    
    private int ASSEMBLY_STATE = 0, WAVE_STATE = 1;
    private int gameState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        DataContainer.game_happend = false; 
    }

    public void StartGame()
    {
        currentWave = 0;
        score = 0;
        time = 0.0f;
        totalTime = 0.0f;
        isGameEnd = false;
        gameState = ASSEMBLY_STATE;

        StartCoroutine(SpawnMaterials());
        startGameButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isGameEnd)
        {
            time += Time.deltaTime;
            // totalTime += Time.deltaTime;
            if (time > assembly_time && gameState == ASSEMBLY_STATE)
            {
                time = 0f;
                gameState = WAVE_STATE;
                StartCoroutine(SpawnEnemies());
            }
            if ((time > wave_time || enemyPool.Count == 0) && gameState == WAVE_STATE)
            {
                currentWave++;
                if (currentWave == totalWaves) 
                {
                    EndGame();
                    return;
                }
                time = 0f;
                gameState = ASSEMBLY_STATE;
                StartCoroutine(SpawnMaterials());
                Debug.Log($"Текущая волна: {currentWave}");
            }
            clockRotation.SetRotation(Time.deltaTime);
        }
    }

    public void EndGame()
    {
        isGameEnd = true;

        enemyPool.DestroyAll();
        towerPool.DestroyAll();
        materialPool.DestroyAll();
        projectilePool.DestroyAll();

        DataContainer.score = score;
        DataContainer.game_happend = true;

        sceneLoader.LoadTops();
    }

    public void AddScore(int addition)
    {
        score += addition;
    }

    private IEnumerator SpawnMaterials()
    {
        for (int i = 0; i < materialNumber[currentWave]; i++)
        {
            materialSpawnSystem.Spawn();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemyNumber[currentWave]; i++)
        {
            enemySpawnSystem.Spawn();
            yield return new WaitForSeconds(1.5f);
        }
    }

    private int GetRemainEnemyNumber()
    {
        return enemyPool.Count;
    }
}
