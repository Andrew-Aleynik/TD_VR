using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public MaterialSpawnSystem materialSpawnSystem;
    public EnemySpawnSystem enemySpawnSystem;
    public ClockRotation clockRotation;
    public GameObjectPool enemyPool = new GameObjectPool(50);
    public GameObjectPool materialPool = new GameObjectPool(100);
    public GameObjectPool towerPool = new GameObjectPool(50);
    public GameObjectPool projectilePool = new GameObjectPool(200);
    public SceneLoader sceneLoader;
    public Base base_component;
    public Button startGameButton;
    public TextMeshPro count_of_waves;


    public int enemyIncrease = 2;
    public float assembly_time = 30f;
    public float wave_time = 40f;


    private int _total_materials = 6;
    private int _total_enemies = 1;
    private int _current_wave = 0;
    private float time;
    private int score;
    private bool isGameEnd = true;
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
        DataContainer.waves = 0;
    }

    public void StartGame()
    {
        score = 0;
        time = 0.0f;
        isGameEnd = false;
        gameState = ASSEMBLY_STATE;

        StartCoroutine(SpawnMaterials());
        startGameButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isGameEnd)
        {
            if (base_component.IsDie)
            {
                EndGame();
                return;
            }
            time += Time.deltaTime;
            if (time > assembly_time && gameState == ASSEMBLY_STATE)
            {
                _current_wave++;
                time = 0f;
                gameState = WAVE_STATE;
                UpdateCount();
                StartCoroutine(SpawnEnemies());
            }
            if ((enemyPool.Count == 0) && gameState == WAVE_STATE)
            {
                _total_enemies += enemyIncrease;
                time = 0f;
                gameState = ASSEMBLY_STATE;
                StartCoroutine(SpawnMaterials());
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
        DataContainer.waves = _current_wave - 1;
        DataContainer.game_happend = true;

        sceneLoader.LoadTops();
    }

    public void AddScore(int addition)
    {
        score += addition;
    }

    private IEnumerator SpawnMaterials()
    {
        for (int i = 0; i < _total_materials; i++)
        {
            materialSpawnSystem.Spawn();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < _total_enemies; i++)
        {
            enemySpawnSystem.Spawn();
            yield return new WaitForSeconds(1.5f);
        }
    }

    private void UpdateCount()
    {
        count_of_waves.text = _current_wave.ToString();
    }
}
