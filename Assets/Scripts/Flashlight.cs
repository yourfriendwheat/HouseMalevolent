using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private GameObject FlashlightLight;
    private bool FlashlightEnabled = false;
    private InputSystem inputSystem;

    void Awake()
    {
        inputSystem = new InputSystem();
        FlashlightLight.SetActive(FlashlightEnabled);

    }

    void OnEnable()
    {
        inputSystem.OnGround.Enable();
        inputSystem.OnGround.OnLight.performed += Lighting;
        inputSystem.OnGround.OnLight.canceled += Lighting;
    }

    void OnDisable()
    {
        inputSystem.OnGround.OnLight.performed -= Lighting;
        inputSystem.OnGround.OnLight.canceled -= Lighting;
        inputSystem.OnGround.Disable();
    }

    public void Lighting(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            FlashlightEnabled = !FlashlightEnabled;
            FlashlightLight.SetActive(FlashlightEnabled);
            Debug.Log("Flashlight toggled: " + FlashlightEnabled);
        }
    }
}
