using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPPR;

namespace Jason
{
    public class PlayerBullet : MonoBehaviour
    {
        public float speed = 10f;
        private Vector3 moveDirection;
        private float lifetime = 5.0f; // The lifetime of the bullet in seconds
        private float elapsedTime = 0.0f; // The elapsed time since the bullet was instantiated
        private float bulletRadius;

        public void SetDirection(Vector3 direction)
        {
            moveDirection = direction;
        }

        public void Start()
        {
            bulletRadius = GetComponent<MeshRenderer>().bounds.extents.x;
        }

        void Update()
        {
            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Destroy the bullet if its lifetime has elapsed
            if (elapsedTime >= lifetime)
            {
                Destroy(gameObject);
                return;
            }

            // Move the bullet in the specified direction
            transform.position = CustomMethod.MoveTowards(transform.position, transform.position + moveDirection, speed * Time.deltaTime);
            DetectEnemyCollision();
        }


        void DetectEnemyCollision()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy"); // Find all enemies with the tag

            foreach (GameObject enemyObj in enemies)
            {
                Enemy enemy = enemyObj.GetComponent<Enemy>(); // Get the Enemy script
                if (enemy == null) continue; // Skip if no script is found

                float enemyRadius = enemy.GetRadius(); // Get enemy size
                float distance = CustomMethod.CalculateDistance(transform.position, enemyObj.transform.position);

                if (distance <= bulletRadius + enemyRadius) // Collision check
                {
                    enemy.TakeDamage(); // Handle enemy hit
                    Destroy(gameObject); // Destroy bullet
                    break;
                }
            }
        }

    }

}

