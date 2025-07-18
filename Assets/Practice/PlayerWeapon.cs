using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField]
    InputAction fireAction;

    [SerializeField]
    GameObject projectile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Test test");
        fireAction.performed += OnFire;
        fireAction.Enable();
    }

    void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire");

        Instantiate(projectile, transform.position, transform.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyMovement>() != null)
        {
            Destroy(collision.gameObject);
            Destroy(this);
        }
    }
}
