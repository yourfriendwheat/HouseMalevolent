using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSystem : MonoBehaviour
{
    public Transform player;
    public AudioSource audioSource;

    public AudioClip grass;
    public AudioClip road;
    public AudioClip wood;

    public float range = 1.0f;
    public float footstepVolume = 1.0f;
    public LayerMask groundLayer;

    public float runRate = 0.5f;
    public float stepRate = 0.3f; // Adjusted to be faster than walking
    private float nextStepTime = 0f;

    private NewPlayerMovement movement;

    private void Start()
    {
        movement = GetComponent<NewPlayerMovement>();

    }

    private void Update()
    {
        if (movement == null) return;

        bool walk = movement.isWalking;
        bool run = movement.isRunning;

        if ((walk || run) && Time.time >= nextStepTime)
        {
            Footstep();
            nextStepTime = Time.time + (run ? runRate : stepRate); // Adjust time for running or walking
        }
    }

    private void Footstep()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.position, Vector3.down, out hit, range, groundLayer))
        {
            if (hit.collider.CompareTag("Grass"))
            {
                PlayFootstepSFX(grass);
            }
            else if (hit.collider.CompareTag("Road"))
            {
                PlayFootstepSFX(road);
            }
            else if (hit.collider.CompareTag("Wood"))
            {
                PlayFootstepSFX(wood);
            }
        }
    }

    private void PlayFootstepSFX(AudioClip audio)
    {
        if (audioSource == null || audio == null) return;

        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(audio, footstepVolume);
    }
}
