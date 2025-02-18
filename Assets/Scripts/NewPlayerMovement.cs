using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;


public class NewPlayerMovement : MonoBehaviour
{
    public float mouseSensitivity = 3f;
    private float xRotation = 0;
    public float moveSpeed = 5f;
    public float jumpStenght = 30f;
    public float runSpeed = 4f;


    public Player_Collision playerCollosion;
    public Transform playerCamera;
    private Rigidbody rb;

    private bool isGrounded;
    private bool isRunning = false;

    private Vector3 runVector;

    public Vector3 moveVector;

    //ActionMap
    public InputSystem inputSystem;
    public GameManager gameManager;

    private PlayerCrouch crouch;


    public void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerCamera.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.rotation = Quaternion.identity;
        rb = this.transform.GetComponent<Rigidbody>();
        playerCollosion = GetComponent<Player_Collision>();
        inputSystem = new InputSystem();
        crouch = GetComponent<PlayerCrouch>();
    }

    void Start()
    {
        OnEnable();
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {


    }

    void FixedUpdate()
    {
        isGrounded = playerCollosion.isGrounded;
        if (moveVector != Vector3.zero)
        {
            Vector3 moveDirection = transform.TransformDirection(moveVector);
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpStenght, ForceMode.Force);
            Debug.Log("yes");

        }
        else
        {
            Debug.Log("NO");
            Debug.Log(isGrounded);

        }
    }

    public void Movement(InputAction.CallbackContext ctx)
    {
        moveVector = ctx.ReadValue<Vector3>();
    }

    public void MouseLook(InputAction.CallbackContext ctx)
    {
        Vector2 lookInput = ctx.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnRunning(InputAction.CallbackContext ctx)
    {
        if (!isRunning)
        {
            isRunning = true;
            moveSpeed = runSpeed;
            Debug.Log("isrunning");
        }
    }

    private void OnRunningCanceled(InputAction.CallbackContext ctx)
    {
        if (isRunning)
        {
            isRunning = false;
            moveSpeed = 5f;
            Debug.Log("isnotrunning");
        }
    }

    public void OnDisable()
    {
        if (inputSystem != null)
        {
            inputSystem.OnGround.Disable();
            crouch.OnDisable();

            inputSystem.OnGround.Jump.performed -= Jump;
            inputSystem.OnGround.Movement.performed -= Movement;
            inputSystem.OnGround.Movement.canceled -= Movement;
            inputSystem.OnGround.Running.performed -= OnRunning;
            inputSystem.OnGround.Running.canceled -= OnRunningCanceled;
            inputSystem.OnGround.Look.performed -= MouseLook;
        }
    }

    public void OnEnable()
    {
        inputSystem.OnGround.Enable();
        crouch.OnEnable();

        inputSystem.OnGround.Jump.performed += Jump;
        inputSystem.OnGround.Movement.performed += Movement;
        inputSystem.OnGround.Movement.canceled += Movement;
        inputSystem.OnGround.Running.performed += OnRunning;
        inputSystem.OnGround.Running.canceled += OnRunningCanceled;

        inputSystem.OnGround.Look.performed += MouseLook;
    }
}
