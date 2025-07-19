using DependencyInjection;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTurretSpawning : MonoBehaviour
{
    [Inject] private PowerManager powerManager = null;

    [SerializeField]
    InputAction placeTurretAction;

    [SerializeField]
    GameObject turretPrefab;

    [SerializeField]
    int spawnCost = 25;

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
    }
}
