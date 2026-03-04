using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ZombieAudio : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip[] idleMoans;
    public AudioClip[] footsteps;
    public AudioClip[] hurt;
    public AudioClip[] death;

    [Header("Tuning")]
    public float idleMinDelay = 3f;
    public float idleMaxDelay = 8f;
    [Range(0f, 1f)] public float idleVolume = 0.8f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 1f; // ensure 3D
        ScheduleNextIdle();
    }

    void ScheduleNextIdle()
    {
        if (idleMoans == null || idleMoans.Length == 0) return;
        float delay = Random.Range(idleMinDelay, idleMaxDelay);
        Invoke(nameof(PlayIdle), delay);
    }

    void PlayIdle()
    {
        PlayRandom(idleMoans, idleVolume);
        ScheduleNextIdle();
    }

    public void PlayFootstep()
    {
        PlayRandom(footsteps, sfxVolume);
    }

    public void PlayHurt()
    {
        PlayRandom(hurt, sfxVolume);
    }

    public void PlayDeath()
    {
        CancelInvoke();
        PlayRandom(death, sfxVolume);
    }

    void PlayRandom(AudioClip[] clips, float vol)
    {
        if (clips == null || clips.Length == 0) return;
        var clip = clips[Random.Range(0, clips.Length)];
        source.PlayOneShot(clip, vol);
    }
}