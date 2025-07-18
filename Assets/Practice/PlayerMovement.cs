using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    InputActionAsset actionMap;

    [SerializeField]
    float speed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        InputAction movementAction = actionMap.FindAction("move");

        Vector2 input2d = movementAction.ReadValue<Vector2>();
        
        Vector3 input = default;
        input.x = input2d.x;
        input.z = input2d.y;

        Vector3 nextPos = transform.position + input * Time.deltaTime * speed;

        if (nextPos == transform.position)
        {
            return;
        }

        transform.LookAt(nextPos);
        transform.position = nextPos;
    }
}
