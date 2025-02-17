using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//written by Jason

namespace Jason
{
    public class HomingBullet : MonoBehaviour
    {
        private float speed;
        private Transform target;
        private float lifetime = 5.0f; // The lifetime of the bullet in seconds
        private float elapsedTime = 0.0f; // The elapsed time since the bullet was instantiated

        public void SetSpeed(float spd)
        {
            speed = spd;
            target = GameObject.FindGameObjectWithTag("Player").transform;
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

            // Move the bullet towards the target
            if (target == null) return;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
