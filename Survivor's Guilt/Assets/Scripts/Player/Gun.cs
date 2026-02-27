using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 25f;
    public float fireRate = 5f;
    public float range = 100f;

    public Camera fpsCam;

    private float nextTimeToFire = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            ZombieHealth target = hit.transform.GetComponent<ZombieHealth>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}

