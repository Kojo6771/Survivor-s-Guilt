using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public float health = 100f;
    public float damage = 10f;
    public float attackRange = 2f;

    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        agent.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage * Time.deltaTime);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    
}
