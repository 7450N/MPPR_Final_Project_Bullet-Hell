using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gabriel
{
    public class EnemyProjectile : MonoBehaviour
    {
        public float speed = 10f; // Speed of the projectile

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
    }
}

