using DependencyInjection;
using UnityEngine;

public class ActivateTurretInteraction : BaseInteractable
{
    [Inject] private PowerManager powerManager = null;

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Activated");
        
        GetComponent<TurretShooting>().Activated = true;
        powerManager.ChangePower(-10);
    }
}
