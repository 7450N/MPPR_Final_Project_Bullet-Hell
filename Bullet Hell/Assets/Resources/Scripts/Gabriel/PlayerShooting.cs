using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gabriel{
    public class PlayerShooting : MonoBehaviour
    {
        public GameObject projectilePrefab;
        public Transform firePoint;
        public float spreadAngle = 15f;

        public void Shoot()
        {
            float currentAngle = Random.Range(spreadAngle, -spreadAngle);
            SpawnProjectile(currentAngle);
        }
        private void SpawnProjectile(float angle)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            projectile.transform.Rotate(0, 0, angle);
        }
    }
}
