using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Jason

namespace Jason
{
    public class WaveBullet : MonoBehaviour
    {
        private float speed;
        private float frequency;
        private float magnitude;
        private Vector3 direction;
        private Vector3 startPosition;
        private float timeElapsed;
        private float lifetime = 5.0f; // The lifetime of the bullet in seconds
        private float elapsedTime = 0.0f; // The elapsed time since the bullet was instantiated

        public void SetParameters(Vector3 dir, float spd, float freq, float mag)
        {
            direction = dir.normalized;
            speed = spd;
            frequency = freq;
            magnitude = mag;
            startPosition = transform.position; // Store initial position
            timeElapsed = 0f; // Reset time counter
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

            // Update the time elapsed for the wave motion
            timeElapsed += Time.deltaTime;

            // Calculate the new position with wave motion
            transform.position = startPosition + (direction * speed * timeElapsed)
                                + (Vector3.up * Mathf.Sin(timeElapsed * frequency) * magnitude);
        }
    }
}
