using System;
using DependencyInjection;
using UnityEngine;
using UnityEngine.UI;

public class TurretUI : MonoBehaviour
{
    [Inject] private PlayerTurretSpawning playerTurretSpawning = null;
    [Inject] private PowerManager powerManager = null;
    [SerializeField] private Image turretIcon = null;
    private void Update()
    {
        turretIcon.color = powerManager.CurrentPower >= playerTurretSpawning.SpawnCost ? Color.white : Color.gray;
    }
}
