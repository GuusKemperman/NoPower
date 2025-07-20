using System;
using DependencyInjection;
using TMPro;
using UnityEngine;

public class PlayerDeathVisual : MonoBehaviour
{
    [Inject] private PlayerHealth playerHealth = null;
    [Inject] private EnemySpawner enemySpawned = null;
    [SerializeField] private GameObject deathVisuals = null;

    [SerializeField] private TextMeshProUGUI elapsedText = null;
    [SerializeField] private TextMeshProUGUI killedEnemiesText = null;
    [SerializeField] private TextMeshProUGUI killedEnemiesCounter = null;
    private int killedEnemies = 0;
    
    private void Awake()
    {
        enemy_behaviour.EnemyDied += AddCounter;
        playerHealth.OnPlayerDied += HandleDeath;
        killedEnemiesCounter.text = "0";
    }
    private void OnDestroy()
    {
        enemy_behaviour.EnemyDied -= AddCounter;
        playerHealth.OnPlayerDied -= HandleDeath; 
    }

    private void AddCounter(enemy_behaviour beh)
    {
        killedEnemies++;
        killedEnemiesCounter.text = $"{killedEnemies}";
    }

    private void HandleDeath()
    {
        deathVisuals.SetActive(true);
        int minutes = (int)enemySpawned.ElapsedTime / 60;
        int seconds = (int)enemySpawned.ElapsedTime % 60;
        elapsedText.text = $"TIME SURVIVED: {minutes}M {seconds:D2}S";
        killedEnemiesText.text = $"ENEMIES KILLED: {killedEnemies}";
    }
}
