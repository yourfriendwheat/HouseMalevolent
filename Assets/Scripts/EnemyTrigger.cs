using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public Transform player;
    public bool m_IsPlayerInRange;
    public bool playerHiding;
    private CrouchingArea inCrouchingArea;

    private void Start()
    {
        inCrouchingArea = GameObject.Find("CrouchingArea").GetComponent<CrouchingArea>();

    }
    private void Update()
    {
        playerHiding = inCrouchingArea.playerCrouching;

        if (m_IsPlayerInRange && playerHiding)
        {
            m_IsPlayerInRange = false; // Player hides after entering, enemy loses sight
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player && !playerHiding)
        {
            m_IsPlayerInRange = true;
        }
        else
        {
            m_IsPlayerInRange = false;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player && !playerHiding)
        {
            m_IsPlayerInRange = false;
        }
    }
}
