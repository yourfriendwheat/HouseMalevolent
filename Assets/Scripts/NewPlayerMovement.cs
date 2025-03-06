using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.AI;


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
    public bool isRunning = false;
    public bool isWalking = false;

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

    Vector2 mouseLook;

    private bool playerwon;
    private bool isPlayerAlive;


    //private Animator StupidPlayer;


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

        //StupidPlayer = this.GetComponent<Animator>();

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
        playerwon = gameManager.PlayerWon;
        isPlayerAlive = gameManager.isPlayerAlive;

        isGrounded = playerCollosion.isGrounded;

        ApplyMouseLook();

        if (playerwon || !isPlayerAlive)
        {
            OnDisable();
        }

        //UpdateAnimation();
    }

    void FixedUpdate()
    {
        if (moveVector != Vector3.zero)
        {
            Vector3 moveDirection = transform.TransformDirection(moveVector);
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void MouseLook(InputAction.CallbackContext ctx)
    {
        // Store input value for processing in FixedUpdate
        mouseLook = ctx.ReadValue<Vector2>();
    }

    private void ApplyMouseLook()
    {
        float mouseX = mouseLook.x * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = mouseLook.y * mouseSensitivity * Time.fixedDeltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
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

        if (moveVector.magnitude > 0.1f && isGrounded)
        {
            isWalking = !isRunning;
        }
        else
        {
            isWalking = false;
        }


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


    private void killGame(InputAction.CallbackContext ctx)
    {
        Application.Quit();
        Debug.Log("Application has closed in this build");
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
            inputSystem.OnGround.KillGame.performed -= killGame;

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
        inputSystem.OnGround.KillGame.performed += killGame;
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

/*    void UpdateAnimation()
    {
        if (moveVector.magnitude > 0.1f)
        {
            StupidPlayer.SetBool("isWalking", !isChasing);
            StupidPlayer.SetBool("idle", false);
        }
        else
        {
            StupidPlayer.SetBool("isWalking", false);
            StupidPlayer.SetBool("idle", true);
        }
    }*/
}
