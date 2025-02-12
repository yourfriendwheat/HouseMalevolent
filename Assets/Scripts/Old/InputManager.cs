using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance
    {
        get { return _instance; }
    }

    private InputSystem inputSystem;

    private void Awake()
    {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } 
        else
        {
            _instance = this;
        }

        inputSystem = new InputSystem();
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return inputSystem.OnGround.Movement.ReadValue<Vector2>();
    }
    public Vector2 GetMouseDelta()
    {
        return inputSystem.OnGround.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumped()
    {
        return inputSystem.OnGround.Jump.triggered;
    }
}