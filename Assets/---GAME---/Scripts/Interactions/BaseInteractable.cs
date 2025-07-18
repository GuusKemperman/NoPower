using UnityEngine;

public class BaseInteractable : MonoBehaviour
{
    public virtual void Interact()
    {
        
    }
    
    public virtual void PlayerEnteredRange(PlayerTag player)
    {
        Debug.Log($"Player entered {gameObject.name} range");
    }

    public virtual void PlayerLeftRange(PlayerTag player)
    {
        Debug.Log($"Player left {gameObject.name} range");
    }
}
