using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouch : MonoBehaviour
{
    // ActionMap
    InputSystem inputSystem;

    private Vector3 crouchScale = new Vector3(1, 0.5f, 1); // Scale for crouching
    private Vector3 playerScale = new Vector3(1, 1f, 1);   // Original scale
    private bool isCrouching = false; // Flag to track crouching state

    private void Awake()
    {
        inputSystem = new InputSystem();
    }

    void Start()
    {
        // Enable the input system and subscribe to the crouch action
        inputSystem.OnGround.Enable();
        inputSystem.OnGround.Crouch.performed += OnCrouchPerformed;
        inputSystem.OnGround.Crouch.canceled += OnCrouchCanceled;
    }

    private void OnCrouchPerformed(InputAction.CallbackContext ctx)
    {
        // Crouch when the key is pressed
        if (!isCrouching)
        {
            isCrouching = true;
            transform.localScale = crouchScale;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        }
    }

    private void OnCrouchCanceled(InputAction.CallbackContext ctx)
    {
        // Stand up when the key is released
        if (isCrouching)
        {
            isCrouching = false;
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }
    }
}
