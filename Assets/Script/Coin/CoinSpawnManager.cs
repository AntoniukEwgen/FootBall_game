using System.Collections;
using UnityEngine;

public class CoinSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float spawnAreaWidth = 5f;
    [SerializeField] private float spawnAreaLength = 5f;
    [SerializeField] private float minSpawnDelay = 1f;
    [SerializeField] private float maxSpawnDelay = 5f;

    private float nextSpawnTimestamp;
    private GameObject activeCoin;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaWidth, 0f, spawnAreaLength));

        if (Time.time >= nextSpawnTimestamp && activeCoin == null)
        {
            Vector3 randomSpawnPoint = GetRandomSpawnPoint();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(randomSpawnPoint, 0.1f);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnCoins()
    {
        while (true)
        {
            if (activeCoin == null)
            {
                Vector3 randomSpawnPoint = GetRandomSpawnPoint();
                activeCoin = Instantiate(coinPrefab, randomSpawnPoint, Quaternion.identity);
                nextSpawnTimestamp = Time.time + Random.Range(minSpawnDelay, maxSpawnDelay);
            }
            yield return new WaitForSeconds(nextSpawnTimestamp - Time.time);
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        return transform.position + new Vector3(
            Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f),
            0f,
            Random.Range(-spawnAreaLength / 2f, spawnAreaLength / 2f)
        );
    }
}