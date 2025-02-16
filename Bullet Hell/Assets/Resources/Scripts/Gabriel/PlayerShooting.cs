using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gabriel
{
    public class PlayerShooting : MonoBehaviour
    {
        public GameObject projectilePrefab; // The projectile object to spawn
        public Transform firePoint; // The point from which the projectile is fired
        public float spreadAngle = 15f; // The range of angle for projectile spread

        // Method to shoot a projectile
        public void Shoot()
        {
            // Generate a random angle within the spread range
            float currentAngle = Random.Range(spreadAngle, -spreadAngle);

            // Spawn the projectile at the fire point with the calculated angle
            SpawnProjectile(currentAngle);
        }

        // Method to instantiate and rotate the projectile
        private void SpawnProjectile(float angle)
        {
            // Instantiate the projectile at the fire point
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Rotate the projectile by the specified angle
            projectile.transform.Rotate(0, 0, angle);
        }
    }
}
