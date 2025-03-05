using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Array of waypoints for the enemy to patrol
    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]
    LayerMask playerLayer;

    //Range within which the enemy can attack the player
    [SerializeField]
    float attackRange = 2f;

    private GameObject player;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private bool isChasing;
    private bool isInRange;
    private bool lose;
    private bool ignore;

    private EnemyTrigger tiggerPlayer;
    public AudioSource enemySound;

    public GameManager GameManager;

    private Animator HotEnemy;


    void Start()
    {
        enemySound = GetComponent<AudioSource>();

        lose = false;

        // Initialize NavMeshAgent and find the player object
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.Find("Player");

        // Start patrolling by moving to a random waypoint
        MoveToRandomWaypoint();

        // Get the EnemyTrigger component attached to a child object
        tiggerPlayer = this.GetComponentInChildren<EnemyTrigger>();

        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        HotEnemy = this.GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the player is within attack range using a Physics sphere check
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        // Check if the player is within detection range using the EnemyTrigger component
        bool isInRange = tiggerPlayer.m_IsPlayerInRange;

        // If the player is within detection range, start chasing
        if (isInRange)
        {
            isChasing = true;
            Chase();

            // If the player is also within attack range, attack
            if (playerInAttackRange)
            {
                lose = true;
                transform.LookAt(player.transform.position);
                Attack();
            }

        }

        // If the enemy was chasing but the player is no longer in range, stop chasing and return to patrolling
        else if (isChasing)
        {
            isChasing = false;
            MoveToRandomWaypoint();

        }

        // If the enemy is not chasing and has reached its current waypoint, move to a new random waypoint
        else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f )
        {
            MoveToRandomWaypoint();
        }

        UpdateAnimation();

    }


    // Moves the enemy to a random waypoint from the waypoints array
    void MoveToRandomWaypoint()
    {
        if (waypoints.Length == 0) return;
        // Choose a random waypoint and set it as the destination
        int randomIndex = Random.Range(0, waypoints.Length);
        navMeshAgent.SetDestination(waypoints[randomIndex].position);
    }

    // Makes the enemy chase the player by setting the player's position as the destination
    void Chase()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
    }

    //Calls the Gameover function
    public void Attack()
    {
        if(lose == true)
        {
            enemySound.Stop();
            GameManager.gameOver();
        }
    }

    void UpdateAnimation()
    {
        float speed = navMeshAgent.velocity.magnitude;

        if (speed > 0.01f)
        {
            HotEnemy.SetBool("isWalking", !isChasing);
            HotEnemy.SetBool("isChasing", isChasing);
            HotEnemy.SetBool("idle", false);
        }
        else
        {
            HotEnemy.SetBool("isWalking", false);
            HotEnemy.SetBool("isChasing", false);
            HotEnemy.SetBool("idle", true);
        }
    }
}
