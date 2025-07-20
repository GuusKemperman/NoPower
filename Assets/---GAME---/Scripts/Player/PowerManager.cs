using System;
using DependencyInjection;
using Unity.Collections;
using UnityEngine;

public class PowerManager : MonoBehaviour,IDependencyProvider
{
    [SerializeField] private int maxPower = 100;
    [SerializeField,ReadOnly] private int currentPower = 100;
    public int MaxPower => maxPower;
    public int CurrentPower => currentPower;

    public event Action<int> OnPowerChange;
    public static event Action OnGainedPower; 
    
    [DependencyInjection.Provide]
    public PowerManager Provide()
    {
        return this;
    }

    private void Start()
    {
        currentPower = -1;
        OnPowerChange?.Invoke(currentPower);
    }

    public void ChangePower(int delta)
    {
        int prevPower = currentPower;
        currentPower = Mathf.Clamp(currentPower + delta, 0, maxPower);

        if (prevPower != currentPower)
        {
            OnPowerChange?.Invoke(currentPower);
        }

        if (prevPower < currentPower)
        {
            OnGainedPower?.Invoke();
        }
    }
}
