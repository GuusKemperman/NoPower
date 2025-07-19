using System;
using DependencyInjection;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTurretSpawning : MonoBehaviour, IDependencyProvider
{
    [Inject] private PowerManager powerManager = null;

    [SerializeField]
    InputAction placeTurretAction;
    
    [SerializeField]
    GameObject turretPrefab;

    [SerializeField]
    int spawnCost = 25;

    public int SpawnCost => spawnCost;
    public static event Action OnPlacedTurret;

    [DependencyInjection.Provide]
    public PlayerTurretSpawning Provide()
    {
        return this;
    }
    
    void Start()
    {
        Debug.Log("Test test");
        placeTurretAction.performed += OnSpawnAttempt;
        placeTurretAction.Enable();
    }

    void OnSpawnAttempt(InputAction.CallbackContext context)
    {
        Debug.Log("Fire");

        if (powerManager.CurrentPower < spawnCost)
        {
            return;
        }

        powerManager.ChangePower(-spawnCost);
        Instantiate(turretPrefab, transform.position + new Vector3(0, 100, 0), transform.rotation);
        OnPlacedTurret?.Invoke();
    }
}
