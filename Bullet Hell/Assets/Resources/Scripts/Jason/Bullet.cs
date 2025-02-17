using MPPR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jason
{
    public class BulletMovement : MonoBehaviour
    {
        private Vector3 moveDirection;
        private float moveSpeed;
        private float lifetime = 5.0f; // The lifetime of the bullet in seconds
        private float elapsedTime = 0.0f; // The elapsed time since the bullet was instantiated
        private float bulletRadius;
        public float bulletDmg = 5; // Damage dealt by the bullet

        private void Start()
        {
            bulletRadius = GetComponent<MeshRenderer>().bounds.extents.x;
        }

        public void SetDirection(Vector3 direction, float speed)
        {
            moveDirection = direction;
            moveSpeed = speed;
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
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            DetectCollision();
        }

        void DetectCollision()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player"); // Find the player with the tag
            Player player = playerObj.GetComponent<Player>(); // Get the Player script
            if (player == null) return; // Skip if no script is found

            float playerRadius = player.GetRadius(); // Get enemy size
            float distance = CustomMethod.CalculateDistance(transform.position, playerObj.transform.position);

            if (distance <= bulletRadius + playerRadius) // Collision check
            {
                player.TakeDamage(bulletDmg); // Handle enemy hit
                Destroy(gameObject); // Destroy bullet
            }
        }
    }
}
