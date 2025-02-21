using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class NewPlayerMovement : MonoBehaviour
{
    public float mouseSensitivity = 3f;
    private float xRotation = 0;
    public float moveSpeed;
    public float jumpStenght = 30f;
    public float runSpeed = 4f;
    private float move_OriginalSpeed;

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


    //stamina
    public Image BoostBar;
    public float Boost, MaxBoost;
    public float RunCost;
    public float ChargeRate;
    private Coroutine recharge;


    public void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerCamera.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.rotation = Quaternion.identity;
        rb = this.transform.GetComponent<Rigidbody>();
        playerCollosion = GetComponent<Player_Collision>();
        inputSystem = new InputSystem();
        crouch = GetComponent<PlayerCrouch>();
        move_OriginalSpeed = moveSpeed;

    }

    void Start()
    {
        OnEnable();
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;

        if (MaxBoost <= 0) MaxBoost = 100f; // Default value if not set
        Boost = MaxBoost;
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
        if (!isRunning && Boost > 0)
        {
            isRunning = true;
            moveSpeed = runSpeed;
            Debug.Log("isrunning");

            if (recharge != null)
            {
                StopCoroutine(recharge); // Stop recharge if running
                recharge = null;
            }

            StartCoroutine(DrainBoost()); // Start draining boost continuously
        }
    }


    private void OnRunningCanceled(InputAction.CallbackContext ctx)
    {
        if (isRunning)
        {
            isRunning = false;
            moveSpeed = move_OriginalSpeed;
            Debug.Log("isnotrunning");

            if (recharge == null)
            {
                recharge = StartCoroutine(RechargeBoost());
            }
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

    private IEnumerator DrainBoost()
    {
        while (isRunning && Boost > 0)
        {
            Boost -= RunCost * Time.deltaTime;
            Boost = Mathf.Clamp(Boost, 0, MaxBoost);
            BoostBar.fillAmount = Boost / MaxBoost;

            if (Boost <= 0)
            {
                isRunning = false;
                moveSpeed = move_OriginalSpeed;
                //Debug.Log("Boost Depleted!");

                if (recharge == null)
                {
                    recharge = StartCoroutine(RechargeBoost());
                }
            }
            yield return null;
        }
    }


    private IEnumerator RechargeBoost()
    {
        yield return new WaitForSeconds(1f); // Delay before recharging starts

        while (Boost < MaxBoost)
        {
            Boost += ChargeRate * Time.deltaTime;
            Boost = Mathf.Clamp(Boost, 0, MaxBoost);
            BoostBar.fillAmount = Boost / MaxBoost;
            yield return null; // Wait for next frame
        }

        recharge = null;
    }


}
