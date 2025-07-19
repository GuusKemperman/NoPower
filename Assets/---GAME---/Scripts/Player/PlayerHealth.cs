using System;
using DependencyInjection;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDependencyProvider
{
    public event Action<int> OnHealthChanged;
    
    [SerializeField] private int maxHealth = 10;
    private int currentHealth = 0;
    
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
    }

    public void ChangeHealth(int delta)
    {
        currentHealth = Mathf.Clamp(currentHealth + delta, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }
}
