using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingArea : MonoBehaviour
{
    public Transform player;
    public bool playerCrouching;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            playerCrouching = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            playerCrouching = false;
        }
    }
}
