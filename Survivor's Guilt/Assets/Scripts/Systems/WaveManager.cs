using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;

    public int round = 1;
    public int zombiesPerRound = 5;

    void Start()
    {
        StartRound();
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Zombie").Length == 0)
        {
            round++;
            zombiesPerRound += 3;
            StartRound();
        }
    }

    void StartRound()
    {
        for (int i = 0; i < zombiesPerRound; i++)
        {
            int randomSpawn = Random.Range(0, spawnPoints.Length);
            Instantiate(zombiePrefab, spawnPoints[randomSpawn].position, Quaternion.identity);
        }
    }
}
