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
    [SerializeField]
    InputActionAsset actionMap;

    private bool paused = false;
    public event Action<PauseState> OnPaused; 
    
    [DependencyInjection.Provide]
    PauseManager Provide()
    {
        return this;
    }    
    private void Update()
    {
        InputAction pauseAction = actionMap.FindAction("Pause");
        if (pauseAction.WasPerformedThisFrame())
        {
            TogglePause();
        }
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
