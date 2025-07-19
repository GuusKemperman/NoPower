using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DependencyInjection;

public class HealthBar : MonoBehaviour
{
    [Inject] private PlayerHealth health = null;
    [SerializeField] private List<Image> bars = new List<Image>();
    
    private void Awake()
    {
        health.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDestroy()
    {
        health.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int number)
    {
        for (int i =0; i < bars.Count;i++)
        {
            if (i < number )
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
