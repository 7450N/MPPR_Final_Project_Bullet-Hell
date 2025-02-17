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
        }
    }
}
