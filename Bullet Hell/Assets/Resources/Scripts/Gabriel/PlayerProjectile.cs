using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gabriel
{
    public class PlayerProjectile : MonoBehaviour
    {
        public float speed = 10f; // Speed at which the projectile moves

        private void Update()
        {
            // Move the projectile upwards at the specified speed
            transform.position += transform.up * speed * Time.deltaTime;
        }

        private void OnBecameInvisible()
        {
            // Destroy the projectile when it goes off-screen
            Destroy(gameObject);
        }
    }
}


