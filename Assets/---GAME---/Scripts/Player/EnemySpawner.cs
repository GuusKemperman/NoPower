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

    [SerializeField]
    int maxEnemiesActive = 40;

    
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

            int enemyCount = GameObject.FindGameObjectsWithTag("enemy").Length;

            if (true)
            {
                if (enemyCount < maxEnemiesActive)
                {
                    GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                    enemy.GetComponent<enemy_behaviour>().player = transform;
                    enemy.GetComponent<enemy_behaviour>().playerOb = gameObject;
                }
            }
            
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
