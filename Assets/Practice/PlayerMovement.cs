using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    InputActionAsset actionMap;

    [SerializeField]
    float speed = 10f;

    // Update is called once per frame
    void Update()
    {
        InputAction movementAction = actionMap.FindAction("Move");

        Vector2 input2d = movementAction.ReadValue<Vector2>();
        
        Vector3 input = default;
        input.x = input2d.x;
        input.z = input2d.y;

        InputAction camAction = actionMap.FindAction("MousePos");

        Vector3 nextPos = transform.position + input * Time.deltaTime * speed;
        Vector2 mousePos = camAction.ReadValue<Vector2>();

        Camera cam = Camera.main;
        Vector3 mouseWorld = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y));

        mouseWorld.y = 0;

        transform.LookAt(mouseWorld);
        transform.position = nextPos;
    }
}
