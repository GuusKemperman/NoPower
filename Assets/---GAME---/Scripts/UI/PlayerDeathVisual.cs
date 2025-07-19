using System;
using DependencyInjection;
using UnityEngine;

public class PlayerDeathVisual : MonoBehaviour
{
    [Inject] private PlayerHealth playerHealth = null;
    [SerializeField] private GameObject deathVisuals = null;
    
    private void Awake()
    {
        playerHealth.OnPlayerDied += HandleDeath;
    }
    private void OnDestroy()
    {
        playerHealth.OnPlayerDied -= HandleDeath; 
    }
    
    private void HandleDeath()
    {
        deathVisuals.SetActive(true);
    }
}
