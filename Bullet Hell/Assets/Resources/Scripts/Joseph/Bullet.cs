using UnityEngine;
using System;

namespace Joseph
{
    public class Bullet : MonoBehaviour
    {
        private Vector2 startPosition;
        private Vector2 direction;
        private float speed;
        private float amplitude;
        private float frequency;
        private float timeElapsed = 0f;
        private float bulletRadius = 0.3f; // Adjusted for bullet scale

        public void InitializeBezierMovement(Vector2 start, Vector2 dir, float amp, float freq)
        {
            startPosition = start;
            direction = NormalizeVector(dir);
            speed = GetVectorMagnitude(dir);
            amplitude = amp;
            frequency = freq;
        }

        void Update()
        {
            timeElapsed += Time.deltaTime;

            // Move along a Bezier wave path
            float waveOffset = CustomSin(timeElapsed * frequency) * amplitude;
            Vector2 perpendicular = new Vector2(-direction.y, direction.x);
            Vector2 curvedPosition = startPosition + direction * (speed * timeElapsed) + perpendicular * waveOffset;

            transform.position = curvedPosition;

            // Check collision with Player
            CheckCollisionWithPlayer();

            // Destroy bullet if it moves too far or if it's been 5 seconds
            if (timeElapsed > 6f)
            {
                Destroy(gameObject);
            }
        }

        void CheckCollisionWithPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float playerRadius = 0.2f; // Adjusted for player scale
                float distance = GetDistance(transform.position, player.transform.position);

                if (distance < bulletRadius + playerRadius)
                {
                    Debug.Log("Bullet hit the Player!");
                    Destroy(player); // Destroy the player
                    Destroy(gameObject); // Destroy the bullet
                }
            }
        }

        private float GetDistance(Vector3 a, Vector3 b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private Vector2 NormalizeVector(Vector2 v)
        {
            float mag = GetVectorMagnitude(v);
            return mag == 0 ? Vector2.zero : new Vector2(v.x / mag, v.y / mag);
        }

        private float GetVectorMagnitude(Vector2 v)
        {
            return (float)Math.Sqrt(v.x * v.x + v.y * v.y);
        }

        private float CustomSin(float x)
        {
            float x2 = x * x;
            return x - (x2 * x) / 6 + (x2 * x2 * x) / 120 - (x2 * x2 * x2 * x) / 5040;
        }
    }

}
