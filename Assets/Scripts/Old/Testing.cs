using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Testing : MonoBehaviour
{

    public float mouseSensitivity = 3f;
    private float xRotation = 0;
    public float moveSpeed = 5f;
    public float jumpStenght = 30f;
    public Player_Collision Player_Collision;
    public Transform playerCamera;
    private Rigidbody rb;
    private bool isGrounded;

    private Vector3 moveVector;

    //ActionMap
    InputSystem inputSystem;


    // Start is called before the first frame update
    private void Awake()
    {
        playerCamera.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.rotation = Quaternion.identity;
        rb = this.transform.GetComponent<Rigidbody>();
        Player_Collision = GetComponent<Player_Collision>();
        inputSystem = new InputSystem();
    }

    void Start()
    {
        inputSystem.OnGround.Enable();
        inputSystem.OnGround.Jump.performed += Jump;
        inputSystem.OnGround.Movement.performed += Movement;
        inputSystem.OnGround.Movement.canceled += Movement;

        inputSystem.OnGround.Look.performed += MouseLook;

        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Player_Collision.isGrounded;
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
}