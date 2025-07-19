using System;
using DependencyInjection;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PauseState
{
    Paused,
    Unpaused
}

public class PauseManager : MonoBehaviour, IDependencyProvider
{
    [Inject] private PlayerHealth playerHealth = null;
    [SerializeField]
    InputActionAsset actionMap;

    private bool paused = false;
    private bool playerDead = false;
    public event Action<PauseState> OnPaused; 
    
    [DependencyInjection.Provide]
    PauseManager Provide()
    {
        return this;
    }

    private void Awake()
    {
        playerHealth.OnPlayerDied += PauseOnDeath;
    }

    private void OnDestroy()
    {
        playerHealth.OnPlayerDied -= PauseOnDeath;
    }

    private void Update()
    {
        InputAction pauseAction = actionMap.FindAction("Pause");
        if (pauseAction.WasPerformedThisFrame() && !playerDead)
        {
            TogglePause();
        }
    }
    
    private void PauseOnDeath()
    {
        playerDead = true;
        Time.timeScale = 0.0f;
    }
    
    private void TogglePause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0.0f : 1.0f;
        OnPaused?.Invoke(paused?PauseState.Paused:PauseState.Unpaused);
    }

    public void SetState(PauseState toSet)
    {
        paused = toSet == PauseState.Paused;
        Time.timeScale = paused ? 0.0f : 1.0f;
        OnPaused?.Invoke(toSet);
    }
}
