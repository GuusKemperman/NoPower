using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DependencyInjection;

public class PowerBar : MonoBehaviour
{
    [Inject] private PowerManager powerManager = null;
    [SerializeField] private List<Image> bars = new List<Image>();
    
    private void Awake()
    {
        powerManager.OnPowerChange += UpdatePowerBar;
    }

    private void OnDestroy()
    {
        powerManager.OnPowerChange -= UpdatePowerBar;
    }

    private void UpdatePowerBar(int number)
    {
        for (int i =0; i < bars.Count;i++)
        {
            if (i* bars.Count < number )
            {
                bars[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                bars[i].color = new Color(1, 1, 1, 0);
            }
        }
    }
}
