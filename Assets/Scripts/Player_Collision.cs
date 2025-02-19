using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Collision : MonoBehaviour
{
    public bool isGrounded;
    public bool getKey = false;
   // public AudioClip keySound;
   // AudioSource audioSource;

    void Start()
    
    {
       // audioSource = GetComponent<AudioSource>();
    }
    


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("CarKey"))
        {
            getKey = true;
           // audioSource.PlayOneShot(keySound);
            Destroy(GameObject.FindWithTag("CarKey"));
        }

        if (collision.gameObject.CompareTag("Car") && getKey == true)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().Win();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
