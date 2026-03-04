using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    private bool isDead;
    private WaveManager waveManager;

    void Start()
    {
        currentHealth = maxHealth;

        // Fallback if WaveManager wasn't injected
        if (waveManager == null)
            waveManager = FindObjectOfType<WaveManager>();
    }

    // Call this from WaveManager right after Instantiate
    public void SetWaveManager(WaveManager wm)
    {
        waveManager = wm;
    }

    // Optional helper for scaling health cleanly
    public void SetMaxHealth(float newMax)
    {
        maxHealth = newMax;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        GetComponent<ZombieAudio>()?.PlayHurt();

        if (currentHealth <= 0f)
        {
            Die();
            GetComponent<ZombieAudio>()?.PlayDeath();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (waveManager != null)
            waveManager.ZombieKilled();
        else
            Debug.LogWarning("Zombie died but WaveManager reference was missing.");

        Destroy(gameObject);
    }
}