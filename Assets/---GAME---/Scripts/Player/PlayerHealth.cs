using System;
using DependencyInjection;
using UnityEditor.VersionControl;
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDependencyProvider
{
    public event Action<int> OnHealthChanged;
    public event Action OnPlayerDied;
    
    [SerializeField] private int maxHealth = 10;
    private int currentHealth = 0;

    [SerializeField] float invincibilityTimeActive= 0.1f;
    private bool Invincible = false;

    private float invincibilityTimer;

    [DependencyInjection.Provide]
    PlayerHealth Provide()
    {
        return this;
    }

    void Start()
    {
        SetHealth(maxHealth);
    }

    public void SetHealth(int health)
    {
        currentHealth = health;
        OnHealthChanged?.Invoke(currentHealth);
        
        if (currentHealth <= 0)
        {
            OnPlayerDied?.Invoke();
        }
    }

    public void Update()
    {

    }

    public void ChangeHealth(int delta)
    {
        if (!Invincible)
        {
            currentHealth = Mathf.Clamp(currentHealth + delta, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
            StartCoroutine(InvincibilityTimer());
        }

        if (currentHealth <= 0)
        {
            OnPlayerDied?.Invoke();
        }
    }

    IEnumerator InvincibilityTimer()
    {
        Invincible = true;
        yield return new WaitForSeconds(invincibilityTimeActive);
        Invincible = false;
    }
}
