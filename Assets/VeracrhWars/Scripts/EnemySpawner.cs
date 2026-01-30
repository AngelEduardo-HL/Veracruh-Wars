using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnRadius = 0.5f;

    public void SpawnEnemies(GameObject enemyPrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0f,
                Random.Range(-spawnRadius, spawnRadius)
            );

            Instantiate(enemyPrefab, transform.position + offset, transform.rotation);
        }
    }
}
