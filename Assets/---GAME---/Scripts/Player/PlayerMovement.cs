using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    InputActionAsset actionMap;

    [SerializeField]
    float speed = 10f;
    
    private Plane rotationPlane = new(Vector3.up, Vector3.zero);
    private Camera camera;
    private CharacterController characterController = null;
    public float Speed => speed;

    private void Start()
    {
        camera = Camera.main;
        rotationPlane = new(Vector3.up, Vector3.zero);
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotationMouse();



    }

    private void HandleMovement()
    {
        InputAction movementAction = actionMap.FindAction("Move");

        Vector2 input2d = movementAction.ReadValue<Vector2>();
        
        Vector3 input = default;
        input.x = input2d.x;
        input.z = input2d.y;
        
        characterController.Move(input * Time.deltaTime * speed);
    }

    private void HandleRotationMouse()
    {
         InputAction camAction = actionMap.FindAction("MousePos");
         Vector2 mousePos = camAction.ReadValue<Vector2>();
         Vector2 mousePosScreen = mousePos;
         Ray ray = camera.ScreenPointToRay(mousePosScreen);
         if (rotationPlane.Raycast(ray, out float distanceToGround))
         {
             Vector3 targetPoint = ray.GetPoint(distanceToGround);
             Vector3 direction = (targetPoint - transform.position).normalized;
             Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
             transform.rotation = targetRotation;
         }
   }
}
