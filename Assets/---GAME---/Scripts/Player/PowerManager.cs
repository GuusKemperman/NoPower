using System;
using DependencyInjection;
using Unity.Collections;
using UnityEngine;

public class PowerManager : MonoBehaviour,IDependencyProvider
{
    [SerializeField] private int maxPower = 100;
    [SerializeField,ReadOnly] private int currentPower = 100;

    public event Action<int> OnPowerChange; 
    
    [DependencyInjection.Provide]
    public PowerManager Provide()
    {
        return this;
    }

    public void ChangePower(int delta)
    {
        currentPower = Mathf.Clamp(currentPower + delta, 0, maxPower);
    }
}
