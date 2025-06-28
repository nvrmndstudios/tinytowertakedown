using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Castle castle;

    [Header("Wave Settings")]
    [SerializeField] private int initialEnemyCount = 3;
    [SerializeField] private int enemyIncreasePerWave = 2;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float timeBetweenEnemies = 0.5f;

    [Header("Looping")]
    [SerializeField] private bool loopWaves = true;

    private int currentWave = 0;
    private bool isGameActive = false;

    public Action<int, int> OnWaveStarted; // (waveNumber, enemyCount)
    private List<GameObject> activeEnemies = new List<GameObject>();
    
    [Header("Distance Thresholds")]
    [SerializeField] private float stage2Radius = 6f;
    [SerializeField] private float stage1Radius = 3f;
    [SerializeField] private float deathRadius = 1f;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        if (isGameActive) return;

        Debug.Log("Game Started");
        isGameActive = true;
        currentWave = 0;

        StartCoroutine(HandleWaves());
    }

    public void EndGame()
    {
        if (!isGameActive) return;

        Debug.Log("Game Ended");
        isGameActive = false;
        StopAllCoroutines();

        activeEnemies.Clear();
    }

    IEnumerator HandleWaves()
    {
        while (isGameActive)
        {
            int enemyCount = CalculateEnemyCount(currentWave);
            OnWaveStarted?.Invoke(currentWave + 1, enemyCount);
            yield return StartCoroutine(SpawnWave(enemyCount));

            // Wait until all enemies are dead
            yield return new WaitUntil(() => activeEnemies.Count == 0);

            currentWave++;

            if (!loopWaves && currentWave > 10)
            {
                EndGame();
                yield break;
            }
        }
    }

    IEnumerator SpawnWave(int count)
    {
        for (int i = 0; i < count && isGameActive; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("Enemy prefab array is empty.");
            return;
        }

        // Limit to top or bottom spawn (Z = ±spawnRadius, X = random offset within radius)
        float x = UnityEngine.Random.Range(-spawnRadius, spawnRadius);
        float z = UnityEngine.Random.value > 0.5f ? spawnRadius : -spawnRadius; // top or bottom only

        Vector3 spawnOffset = new Vector3(x, 0, z);
        Vector3 spawnPos = castle.transform.position + spawnOffset;

        GameObject prefabToSpawn = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)];
        GameObject enemy = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        var targetable = enemy.GetComponent<Targetable>();
        targetable.SetCenterPoint(castle, stage2Radius, stage1Radius, deathRadius);
        targetable.OnDeath += () => activeEnemies.Remove(enemy);

        activeEnemies.Add(enemy);
    }

    int CalculateEnemyCount(int wave)
    {
        return initialEnemyCount + wave * enemyIncreasePerWave;
    }

    void OnDrawGizmosSelected()
    {
        if (castle.transform != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(castle.transform.position, spawnRadius);
        }
    }
}
