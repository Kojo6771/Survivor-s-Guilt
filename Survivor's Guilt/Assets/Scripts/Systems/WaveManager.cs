using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;

    public int roundNumber = 1;
    public int baseZombieCount = 5;

    private int zombiesAlive;

    void Start()
    {
        StartRound();
    }

    void Update()
    {
        if (zombiesAlive <= 0)
        {
            roundNumber++;
            StartRound();
        }
    }

    void StartRound()
    {
        int zombiesToSpawn = baseZombieCount + (roundNumber * 2);
        zombiesAlive = zombiesToSpawn;

        for (int i = 0; i < zombiesToSpawn; i++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject zombie = Instantiate(zombiePrefab, spawn.position, Quaternion.identity);

            zombie.GetComponent<ZombieHealth>().maxHealth += roundNumber * 10;
            zombie.GetComponent<ZombieHealth>().TakeDamage(0);
        }
    }

    public void ZombieKilled()
    {
        zombiesAlive--;
    }
}
