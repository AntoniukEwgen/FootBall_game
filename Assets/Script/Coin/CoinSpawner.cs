using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float squareWidth = 5f;
    public float squareLength = 5f;
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 5f;

    private float nextSpawnTime;
    private GameObject currentCoin;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(squareWidth, squareLength, 0f));

        if (Time.time >= nextSpawnTime && currentCoin == null)
        {
            Vector3 randomSpawnPoint = transform.position + new Vector3(
                Random.Range(-squareWidth / 2f, squareWidth / 2f),
                0f,
                Random.Range(-squareLength / 2f, squareLength / 2f)
            );
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(randomSpawnPoint, 0.1f);

            nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    private void Start()
    {
        SpawnPrefab();
    }

    private void SpawnPrefab()
    {
        if (currentCoin == null)
        {
            Vector3 randomSpawnPoint = transform.position + new Vector3(
                Random.Range(-squareWidth / 2f, squareWidth / 2f),
                0f,
                Random.Range(-squareLength / 2f, squareLength / 2f)
            );
            currentCoin = Instantiate(prefabToSpawn, randomSpawnPoint, Quaternion.identity);

            nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    private void Update()
    {
        SpawnPrefab();
    }
}
