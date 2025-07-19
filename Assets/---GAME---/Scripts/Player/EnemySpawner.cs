using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable]
public class EnemyStats
{
    public int TimePassage;
    public int Health;
    public int Damage;
    public int MovementSpeed;
}

public class EnemySpawner : MonoBehaviour, DependencyInjection.IDependencyProvider
{
    public float ElapsedTime => timePassed;
    
    [SerializeField]
    float spawnRadius = 20;

    [SerializeField]
    float spawnInterval = 1;

    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    int maxEnemiesActive = 40;

    [SerializeField] private List<EnemyStats> stats = new List<EnemyStats>();
    [SerializeField] private AnimationCurve intervalCurve;
    private float timePassed;
    private float evaulationTimer = 5.0f;
    
    [DependencyInjection.Provide]
    public EnemySpawner Provide()
    {
        return this;
    }
    
    void Start()
    {
        StartCoroutine(SpawnCycle());
        spawnInterval = intervalCurve.Evaluate(timePassed);
    }

    private void Update()
    {
        timePassed = Mathf.Clamp(timePassed+Time.deltaTime,0.0f,float.MaxValue);
        evaulationTimer -= Time.deltaTime;
        
        if (evaulationTimer <= 0.0f)
        {
            spawnInterval = intervalCurve.Evaluate(timePassed);
            evaulationTimer = 5.0f;
        }
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
                    EnemyStats foundStat = stats.FirstOrDefault(x => x.TimePassage >= timePassed) ?? stats[^1];

                    GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

                    enemy_behaviour enemyBehaviour = enemy.GetComponent<enemy_behaviour>();
                    enemyBehaviour.player = transform;
                    enemyBehaviour.playerOb = gameObject;
                    enemyBehaviour.Damage = foundStat.Damage;
                    enemyBehaviour.Health = foundStat.Health;
                    enemyBehaviour.GetComponent<NavMeshAgent>().speed = foundStat.MovementSpeed;
                }
            }
            
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
