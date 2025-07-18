using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement playerMovement = null;
    private float movementSpeed = 0.0f;
    private Animator animator = null;
    private CharacterController characterController = null;

    private int movementAnim = Animator.StringToHash("Movement");
    
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        movementSpeed = playerMovement.Speed;
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        animator.SetFloat(movementAnim,characterController.velocity.magnitude / movementSpeed);
    }
}
