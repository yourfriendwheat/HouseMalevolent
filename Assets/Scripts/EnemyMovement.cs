using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]
    LayerMask playerLayer;

    [SerializeField]
    float sightRange = 10f;


    private GameObject player;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private bool isChasing;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.Find("Player");
        MoveToRandomWaypoint();
    }

    void Update()
    {
        bool playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);

        if (playerInSight)
        {
            isChasing = true;
            Chase();
        }
        else if (isChasing)
        {
            isChasing = false;
            MoveToRandomWaypoint();
        }
        else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            MoveToRandomWaypoint();
        }
    }

    void MoveToRandomWaypoint()
    {
        if (waypoints.Length == 0) return;

        int randomIndex = Random.Range(0, waypoints.Length);
        navMeshAgent.SetDestination(waypoints[randomIndex].position);
    }

    void Chase()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
    }

/*    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }*/
}