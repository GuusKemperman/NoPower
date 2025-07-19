using System;
using System.Collections.Generic;
using DependencyInjection;
using UnityEngine;

public class PauseVisuals : MonoBehaviour
{
    [Inject] private PauseManager pauseManager = null;
    [Inject] private PlayerHealth playerHealth = null;
    [SerializeField] private List<GameObject> showOnPause = new List<GameObject>();
    [SerializeField] private List<GameObject> hideOnPause = new List<GameObject>();
    private void Awake()
    {
        pauseManager.OnPaused += HandlePause;
        playerHealth.OnPlayerDied += HandlePlayerDeath;
    }

    private void OnDestroy()
    {
        playerHealth.OnPlayerDied -= HandlePlayerDeath;
        pauseManager.OnPaused -= HandlePause;
    }

    private void HandlePlayerDeath()
    {
        foreach (GameObject go in showOnPause)
        {
            go.SetActive(false);
        }
            
        foreach (GameObject go in hideOnPause)
        {
            go.SetActive(true);
        }
    }

    private void HandlePause(PauseState obj)
    {
        if (obj == PauseState.Paused)
        {
            foreach (GameObject go in showOnPause)
            {
                go.SetActive(true);
            }
            
            foreach (GameObject go in hideOnPause)
            {
                go.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject go in showOnPause)
            {
                go.SetActive(false);
            }
            
            foreach (GameObject go in hideOnPause)
            {
                go.SetActive(true);
            }
        }
    }
}
