using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnEntry
{
    public GameObject enemyPrefab;
    [Min(1)] public int count = 5;
    public float spawnDelay = 0f;
}

[System.Serializable]
public class WaveData
{
    public List<EnemySpawnEntry> enemies = new();
    public float spawnInterval = 0.5f;
    [Tooltip("0 = use global timeBetweenWaves")]
    public float timeBetweenWavesOverride = 0f;
}

public class WaveSpawner : MonoBehaviour
{
    [Header("Waves")]
    [SerializeField] List<WaveData> waves = new();
    [SerializeField] float timeBetweenWaves = 5f;
    [SerializeField] bool loopWaves = false;

    int waveIndex;
    float countdown = 3f;
    bool isSpawning;
    bool gameEnded;

    void OnEnable() => GameManager.Instance.GameEnded += OnGameEnded;
    void OnDisable() => GameManager.Instance.GameEnded -= OnGameEnded;


    public event Action<int, int> WaveChanged;

public int TotalWaves => waves.Count;

public int CurrentWaveNumber
{
    get
    {
        if (waves.Count == 0)
            return 0;

        return Mathf.Clamp(waveIndex + 1, 1, waves.Count);
    }
}

void Start()
{
    NotifyWaveChanged();
}

    void OnGameEnded(bool isGameWon)
    {
        gameEnded = true;
    }

    void Update()
    {
        if (isSpawning || gameEnded) return;

        countdown -= Time.deltaTime;
        if (countdown <= 0f)
            StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        if (waves.Count == 0 || waveIndex >= waves.Count)
        {
            isSpawning = false;
            yield break;
        }

    isSpawning = true;
    NotifyWaveChanged();

    WaveData wave = waves[waveIndex];

        foreach (EnemySpawnEntry entry in wave.enemies)
        {
            if (entry.enemyPrefab == null)
            {
                Debug.LogWarning($"Wave {waveIndex}: missing enemy prefab!");
                continue;
            }

            if (entry.spawnDelay > 0f)
                yield return new WaitForSeconds(entry.spawnDelay);

            for (int i = 0; i < entry.count; i++)
            {
                Instantiate(entry.enemyPrefab, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(wave.spawnInterval);
            }
        }

        // next wave
        waveIndex++;
NotifyWaveChanged();
        if (waveIndex >= waves.Count)
        {
            if (loopWaves)
            {
                waveIndex = 0;
            }
            else
            {
                isSpawning = false;
                StartCoroutine(WaitForLastEnemies());
                yield break;
            }
        }

        float delay = wave.timeBetweenWavesOverride > 0f
            ? wave.timeBetweenWavesOverride
            : timeBetweenWaves;

        countdown = delay;
        isSpawning = false;
    }

    IEnumerator WaitForLastEnemies()
    {
        yield return new WaitUntil(() => GameManager.Instance.EnemiesAlive <= 0);
        if (!gameEnded)
            GameManager.Instance.WinGame();
    }

    void NotifyWaveChanged()
{
    WaveChanged?.Invoke(CurrentWaveNumber, TotalWaves);
}

}