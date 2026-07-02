using System.Collections;
using UnityEngine;

public class EndlessWaveSpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float timeBetweenWaves = 5f;
    [SerializeField] int enemiesPerWave = 5;
    [SerializeField] float spawnInterval = 0.5f;

    float countdown = 3f;
    int waveNumber;

    void Update()
    {
        if (countdown > 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        waveNumber++;
        for (int i = 0; i < enemiesPerWave + waveNumber; i++)
        {
            // Wróg w Start() i tak ustawia się na początku ścieżki, więc pozycja spawnu nie jest krytyczna
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}