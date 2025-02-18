using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Collision : MonoBehaviour
{
    public bool isGrounded;
    public bool getKey = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Key"))
        {
            getKey = true;
            Destroy(GameObject.FindWithTag("Key"));
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
