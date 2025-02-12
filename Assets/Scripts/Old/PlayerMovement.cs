using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Require a CharacterController component to be attached to the GameObject
[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 3f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    private Vector3 moveDirection = Vector3.zero; // Stores the player's movement direction
    private float rotationX = 0; // Stores the vertical rotation of the camera
    private CharacterController characterController;

    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Calculate forward and right movement directions based on the player's rotation
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Check if the player is holding the Left Shift key to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Calculate movement speed based on whether the player is running or walking
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        
        // Store the current vertical movement direction (for jumping and gravity)
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Handle jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity if the player is not grounded
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the player using the CharacterController
        characterController.Move(moveDirection * Time.deltaTime);


        // Handle camera rotation if the player can move
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}