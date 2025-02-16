using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gabriel
{
    public class EnemyBulletPattern1 : MonoBehaviour
    {
        public GameObject projectilePrefab; // Projectile to spawn
        public Transform firePoint; // Firing position
        private float currentAngle = 0; // Tracks the current firing angle
        public float angle = 130; // Angle between projectiles
        public float bulletsPerSecond = 10f; // Fire rate
        public bool isShooting = false; // Controls shooting state

        public void Start()
        {
            // Start shooting coroutine
            StartCoroutine(ShootContinuously());
        }

        public void ToggleShooting(bool enable)
        {
            // Enable or disable shooting
            isShooting = enable;
        }

        private void Shoot()
        {
            // Alternate between firing angles
            currentAngle += angle;
            SpawnProjectile(currentAngle);
            currentAngle = -currentAngle;
            SpawnProjectile(currentAngle);
            currentAngle = -currentAngle;
        }

        private void SpawnProjectile(float angle)
        {
            // Instantiate a projectile and rotate it
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

                // Wait based on fire rate
                yield return new WaitForSeconds(1f / bulletsPerSecond);
            }
        }
    }
}

