using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement playerMovement = null;
    private CharacterController controller = null;
    private float movementSpeed = 0.0f;
    private Animator animator = null;

    private int movementAnim = Animator.StringToHash("Movement");
    private Rigidbody rb = null;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
        movementSpeed = playerMovement.Speed;
    }

    void Update()
    {
        animator.SetFloat(movementAnim, controller.velocity.magnitude / playerMovement.Speed);
    }
}
