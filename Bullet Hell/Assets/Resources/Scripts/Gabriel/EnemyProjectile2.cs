using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Gabriel
{
    public class EnemyProjectile2 : MonoBehaviour
    {
        public float speed = 10f; // Speed of the projectile
        public GameObject projectilePrefab; // Prefab for spawned projectiles
        public int amountOfProjectiles; // Number of projectiles to split into
        private float currentAngle = 0; // Current angle for projectile spread

        private void Start()
        {
            // Set a random initial angle for projectile spread
            currentAngle = Random.Range(1, 360 / amountOfProjectiles);
        }

        private void Update()
        {
            // Move the projectile upwards based on its speed and frame time
            transform.position += transform.up * speed * Time.deltaTime;
        }

        private void OnBecameInvisible()
        {
            // Destroy the projectile when it goes off-screen
            Destroy(gameObject);
        }

        public void Split()
        {
            // Split the projectile into multiple smaller projectiles
            for (int i = 0; i <= amountOfProjectiles; i++)
            {
                SpawnProjectile(currentAngle);
                currentAngle += (360 / amountOfProjectiles); // Spread projectiles evenly
            }
        }

        private void SpawnProjectile(float angle)
        {
            // Spawn a new projectile and set its rotation based on the angle
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.transform.Rotate(0, 0, angle);
        }

        private void OnDestroy()
        {
            // Split the projectile when it's destroyed
            Split();
        }
    }
}

