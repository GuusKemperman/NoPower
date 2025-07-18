using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    float spawnRadius = 20;

    [SerializeField]
    float spawnInterval = 1;

    [SerializeField]
    GameObject enemyPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnCycle());
    }

    IEnumerator SpawnCycle()
    {
        while (true)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            Vector3 spawnPos = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
            spawnPos *= spawnRadius;
            spawnPos.x += transform.position.x;
            spawnPos.z += transform.position.z;

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
