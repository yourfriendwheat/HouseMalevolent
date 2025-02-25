using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private GameObject FlashlightLight;
    private bool FlashlightEnabled = false;
    private InputSystem inputSystem;

    public AudioClip FlashlightSoundOn;
    public AudioClip FlashlightSoundOff;
    AudioSource audioSource;

    public Image LightBar;
    public float CurrentLighting, MaxLight;
    public float LightCost;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        inputSystem = new InputSystem();
        FlashlightLight.SetActive(FlashlightEnabled);
        if (MaxLight <= 0) MaxLight = 100f; // Default value if not set
        CurrentLighting = MaxLight;
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
        if (ctx.performed && CurrentLighting != 0)
        {
            FlashlightEnabled = !FlashlightEnabled;
            FlashlightLight.SetActive(FlashlightEnabled);
            Debug.Log("Flashlight toggled: " + FlashlightEnabled);
            StartCoroutine(DrainBoost());

            if(FlashlightEnabled)
            {
                audioSource.PlayOneShot(FlashlightSoundOn);

            }
            else
            {
                audioSource.PlayOneShot(FlashlightSoundOff);
            }
        }
    }

    private IEnumerator DrainBoost()
    {
        while (FlashlightEnabled && CurrentLighting > 0)
        {
            CurrentLighting -= LightCost * Time.deltaTime;
            CurrentLighting = Mathf.Clamp(CurrentLighting, 0, MaxLight);
            LightBar.fillAmount = CurrentLighting / MaxLight;

            if (CurrentLighting <= 0)
            {
                FlashlightLight.SetActive(false);
            }
            yield return null;
        }
    }
}
