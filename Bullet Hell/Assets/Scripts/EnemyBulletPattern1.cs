using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPattern1 : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    private Coroutine shootingCoroutine;
    private float currentAngle = 0;
    public float angle = 130;
    public float bulletsPerSecond = 10f;
    private Enemy enemy; // Private field to hold reference

    private void Start()
    {
        enemy = GetComponent<Enemy>(); // Ensure enemy is assigned before using it

        if (enemy == null)
        {
            Debug.LogError("EnemyBulletPattern1: No Enemy component found on " + gameObject.name);
            return; // Exit if there's no Enemy script attached
        }

        shootingCoroutine = StartCoroutine(ShootContinuously());
    }

    private void Update()
    {
        if (enemy != null && enemy.enemyDead && shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null; // Prevent multiple stops
        }
    }

    public void Shoot()
    {
        currentAngle += angle;
        SpawnProjectile(currentAngle);
        currentAngle = -currentAngle;
        SpawnProjectile(currentAngle);
        currentAngle = -currentAngle;
    }

    private void SpawnProjectile(float angle)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.transform.Rotate(0, 0, angle);
    }

    private IEnumerator ShootContinuously()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(1f / bulletsPerSecond);
        }
    }
}

