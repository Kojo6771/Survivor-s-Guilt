using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject zombiePrefab;
    public Transform spawnPointsRoot; // Assign SpawnPoints parent in Inspector (recommended)
    public float timeBetweenSpawns = 0.2f;

    [Header("Wave Settings")]
    public int roundNumber = 1;
    public int baseZombieCount = 5;
    public int zombiesPerRoundMultiplier = 2;

    [Header("Zombie Scaling")]
    public float baseZombieHealth = 100f;
    public float healthIncreasePerRound = 10f;

    private readonly List<Transform> spawnPoints = new List<Transform>();
    private int zombiesAlive;
    private bool roundInProgress;
    private Coroutine spawnRoutine;

    void Awake()
    {
        // Fallback to Find if not assigned
        if (spawnPointsRoot == null)
        {
            GameObject rootObj = GameObject.Find("SpawnPoints");
            if (rootObj != null) spawnPointsRoot = rootObj.transform;
        }

        if (spawnPointsRoot == null)
        {
            Debug.LogError("WaveManager: No SpawnPoints root set/found. Create an object named 'SpawnPoints' OR assign spawnPointsRoot in Inspector.");
            enabled = false;
            return;
        }

        if (zombiePrefab == null)
        {
            Debug.LogError("WaveManager: Zombie Prefab is not assigned.");
            enabled = false;
            return;
        }

        // Collect children only (NOT the parent)
        spawnPoints.Clear();
        for (int i = 0; i < spawnPointsRoot.childCount; i++)
            spawnPoints.Add(spawnPointsRoot.GetChild(i));

        if (spawnPoints.Count == 0)
        {
            Debug.LogError("WaveManager: SpawnPoints root has no child spawn points. Add children under SpawnPoints.");
            enabled = false;
        }
    }

    void Start()
    {
        StartRound();
    }

    void Update()
    {
        if (roundInProgress && zombiesAlive <= 0)
        {
            roundInProgress = false;
            roundNumber++;
            StartRound();
        }
    }

    void StartRound()
    {
        int zombiesToSpawn = baseZombieCount + (roundNumber * zombiesPerRoundMultiplier);
        zombiesAlive = zombiesToSpawn;
        roundInProgress = true;

        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        spawnRoutine = StartCoroutine(SpawnWave(zombiesToSpawn));
    }

    IEnumerator SpawnWave(int zombiesToSpawn)
    {
        for (int i = 0; i < zombiesToSpawn; i++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject zombie = Instantiate(zombiePrefab, spawn.position, spawn.rotation);

            // Hook zombie to this wave manager + scale health
            ZombieHealth zh = zombie.GetComponent<ZombieHealth>();
            if (zh != null)
            {
                zh.SetWaveManager(this);
                float scaledHealth = baseZombieHealth + (roundNumber * healthIncreasePerRound);
                zh.SetMaxHealth(scaledHealth);
            }
            else
            {
                Debug.LogWarning("WaveManager: Spawned zombie has no ZombieHealth component.");
            }

            if (timeBetweenSpawns > 0f)
                yield return new WaitForSeconds(timeBetweenSpawns);
            else
                yield return null;
        }
    }

    public void ZombieKilled()
    {
        zombiesAlive = Mathf.Max(0, zombiesAlive - 1);
    }
}