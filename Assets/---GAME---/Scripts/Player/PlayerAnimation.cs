using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement playerMovement = null;
    private float movementSpeed = 0.0f;
    private Animator animator = null;

    private int movementAnim = Animator.StringToHash("Movement");
    
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        movementSpeed = playerMovement.Speed;
    }

    void Update()
    {
        animator.SetFloat(movementAnim, playerMovement.velocity.magnitude / playerMovement.Speed);
    }
}
