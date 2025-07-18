using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float playerInteractionDistance = 5.0f;
    [SerializeField] private InputActionAsset actionMap = null;
    
    private List<BaseInteractable> detectedInteractables = new List<BaseInteractable>();
    private PlayerTag playerTag = null;
    private BaseInteractable detectedInteractable = null;

    void Awake()
    {
        playerTag = GetComponent<PlayerTag>();
    }
    
    void Update()
    {
        DetectInteractables();
        InputAction interactAction = actionMap.FindAction("Interact");
        if (interactAction.WasPressedThisFrame())
        {
            if (detectedInteractable == null) return;
            detectedInteractable.Interact();
        }
    }

    private void DetectInteractables()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, playerInteractionDistance);

        if (colliders.Length == 0)
        {
            detectedInteractable?.PlayerLeftRange(playerTag);
            detectedInteractable = null;
            return;
        }
        
        Collider[] ineractables = colliders.Where(x => x.GetComponent<BaseInteractable>()).OrderBy(x=>Vector3.Distance(transform.position,x.transform.position)).ToArray();

        if (ineractables.Length == 0)
        {
            detectedInteractable?.PlayerLeftRange(playerTag);
            detectedInteractable = null;
            return;
        }
        
        BaseInteractable closest = ineractables[0].GetComponent<BaseInteractable>();
        if (detectedInteractable == closest) return;
        if (detectedInteractable != null)
        {
            detectedInteractable.PlayerLeftRange(playerTag);
        }

        detectedInteractable = closest;
        detectedInteractable.PlayerEnteredRange(playerTag);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
        Gizmos.DrawSphere(transform.position,playerInteractionDistance);
    }
}
