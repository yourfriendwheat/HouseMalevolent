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

    private NewPlayerMovement NewPlayerMovement;

    [SerializeField] private Transform headCheckPoint; // Empty GameObject placed at the top of the player's head
    [SerializeField] private float headCheckRadius = 0.3f; // Radius for the head check
    [SerializeField] private LayerMask obstacleLayer; // Layer to check for obstacles

    private void Awake()
    {
        inputSystem = new InputSystem();
        NewPlayerMovement = GetComponent<NewPlayerMovement>();
    }

    void Start()
    {
        OnEnable();
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
        // Check if there's enough space to stand up
        if (!Physics.CheckSphere(headCheckPoint.position, headCheckRadius, obstacleLayer))
        {
            isCrouching = false;
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }
        else
        {
            Debug.Log("Can't stand up! Something is above.");
        }
    }

    public void OnDisable()
    {
        inputSystem.OnGround.Disable();
        inputSystem.OnGround.Crouch.performed -= OnCrouchPerformed;
        inputSystem.OnGround.Crouch.canceled -= OnCrouchCanceled;
    }

    public void OnEnable()
    {
        inputSystem.OnGround.Enable();
        inputSystem.OnGround.Crouch.performed += OnCrouchPerformed;
        inputSystem.OnGround.Crouch.canceled += OnCrouchCanceled;
    }
}
