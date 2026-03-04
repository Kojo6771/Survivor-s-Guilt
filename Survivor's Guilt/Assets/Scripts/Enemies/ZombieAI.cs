using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public float attackRange = 2f;
    public float damage = 10f;

    [Header("Footstep Settings")]
    public float stepInterval = 0.5f;

    private float stepTimer;

    private NavMeshAgent agent;
    private Animator animator;
    private ZombieAudio zombieAudio;

    private Transform player;
    private PlayerHealth playerHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        zombieAudio = GetComponent<ZombieAudio>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("No GameObject with tag 'Player' found. Make sure your player is tagged Player.");
            enabled = false;
            return;
        }

        player = playerObj.transform;

        playerHealth = playerObj.GetComponent<PlayerHealth>();
        if (playerHealth == null) playerHealth = playerObj.GetComponentInChildren<PlayerHealth>();
        if (playerHealth == null) playerHealth = playerObj.GetComponentInParent<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component not found on the Player object (or its children/parent).");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);

            HandleFootsteps(true);
        }
        else
        {
            agent.isStopped = true;

            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);

            HandleFootsteps(false);

            playerHealth.TakeDamage(damage * Time.deltaTime);
        }
    }

    void HandleFootsteps(bool isMoving)
    {
        if (!isMoving)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            if (zombieAudio != null)
            {
                zombieAudio.PlayFootstep();
            }

            stepTimer = stepInterval;
        }
    }
}