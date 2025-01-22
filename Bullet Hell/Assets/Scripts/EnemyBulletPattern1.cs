using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPattern1 : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    float currentAngle = 0;
    public float angle = 130;
    public float bulletsPerSecond = 10f;

    private void Start()
    {
        StartCoroutine(ShootContinuously());
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

