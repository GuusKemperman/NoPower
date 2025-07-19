using System;
using DependencyInjection;
using UnityEditor.VersionControl;
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDependencyProvider
{
    public event Action<int> OnHealthChanged;
    
    [SerializeField] private int maxHealth = 10;
    private int currentHealth = 0;

    [SerializeField] float invincibilityTimeActive= 1;
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
    }

    public void Update()
    {
        if (Invincible)
        {
            Debug.Log("invinsible");
        }

        
    }

    public void ChangeHealth(int delta)
    {
        if (!Invincible)
        {
            currentHealth = Mathf.Clamp(currentHealth + delta, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);

            StartCoroutine(InvincibilityTimer());
        }
    }

    IEnumerator InvincibilityTimer()
    {

        Invincible = true;
        yield return new WaitForSeconds(invincibilityTimeActive); 
        Invincible = false;
    }
}
