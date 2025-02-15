using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPattern1 : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    private float currentAngle = 0;
    public float angle = 130;
    public float bulletsPerSecond = 10f;
    public bool isShooting = false;

    public void Start()
    {
        StartCoroutine(ShootContinuously());
    }
    public void ToggleShooting(bool enable)
    {
        isShooting = enable;
    }

    private void Shoot()
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
            if (isShooting)
            {
                Shoot();
            }

            yield return new WaitForSeconds(1f / bulletsPerSecond);
        }
    }
}


