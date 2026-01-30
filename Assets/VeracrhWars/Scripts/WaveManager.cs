using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("Enemy Spawn")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxEnemiesOnMap = 20;
    [SerializeField] private int maxPerSpawn = 5;

    [Header("Spawners")]
    [SerializeField] private EnemySpawner[] spawners;

    private int _aliveEnemies = 0;
    private Coroutine _loop;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        if (spawners == null || spawners.Length == 0)
            spawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);

        _loop = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            TrySpawnBatch();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void TrySpawnBatch()
    {
        if (enemyPrefab == null) return;
        if (spawners == null || spawners.Length == 0) return;
        if (_aliveEnemies >= maxEnemiesOnMap) return;

        int freeSlots = maxEnemiesOnMap - _aliveEnemies;
        int batch = Mathf.Min(maxPerSpawn, freeSlots);

        // elegir un spawner al azar
        EnemySpawner sp = spawners[Random.Range(0, spawners.Length)];
        sp.SpawnEnemies(enemyPrefab, batch);

        _aliveEnemies += batch;
    }

    public void UnregisterEnemy()
    {
        _aliveEnemies = Mathf.Max(0, _aliveEnemies - 1);
    }
}
