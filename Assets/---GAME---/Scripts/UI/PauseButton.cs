using System;
using DependencyInjection;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [Inject] private PauseManager pauseManager = null;
    [SerializeField] private PauseState toSet;

    private void Awake()
    {
        if (pauseManager == null)
        {
            pauseManager = FindAnyObjectByType<PauseManager>();
        }
    }

    public void OnClick()
    {
        pauseManager.SetState(toSet);
    }
}
