using UnityEngine;
using System;

namespace Joseph
{
    public class DVDBullet : MonoBehaviour
    {
        private Vector2 direction;
        private float speed = 5f;
        private float timeElapsed = 0f;
        private float bulletRadius = 0.3f;

        private float minX, maxX, minY, maxY;

        private Camera mainCamera;

        public void Initialize(Vector2 dir, float spd)
        {
            direction = NormalizeVector(dir);
            speed = spd;
        }

        void Start()
        {
            mainCamera = Camera.main;

            // Calculate screen boundaries based on orthographic camera size.
            float screenHeight = mainCamera.orthographicSize;
            float screenWidth = screenHeight * mainCamera.aspect;

            minX = -screenWidth;
            maxX = screenWidth;
            minY = -screenHeight;
            maxY = screenHeight;

            // Randomize the initial direction.
            float angle = (float)(new System.Random().NextDouble() * 2 * Math.PI);
            direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        void Update()
        {
            timeElapsed += Time.deltaTime;

            Vector2 movement = direction * speed * Time.deltaTime;
            transform.position += new Vector3(movement.x, movement.y, 0);

            CheckBounce();

            if (timeElapsed > 10f)
            {
                Destroy(gameObject);
            }

            CheckCollisionWithPlayer();
            CheckCollisionWithShield();
        }

        // Checks if the bullet hits screen boundaries and reflects it.
        void CheckBounce()
        {
            Vector3 pos = transform.position;

            // Reflect direction if bullet hits horizontal bounds.
            if (pos.x <= minX || pos.x >= maxX)
            {
                direction.x *= -1; // Reverse x direction.
                transform.position = new Vector3(Clamp(pos.x, minX, maxX), pos.y, pos.z);
            }

            if (pos.y <= minY || pos.y >= maxY)
            {
                direction.y *= -1; // Reverse y direction.
                transform.position = new Vector3(pos.x, Clamp(pos.y, minY, maxY), pos.z);
            }
        }

        void CheckCollisionWithPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float playerRadius = 0.2f;
                float distance = GetDistance(transform.position, player.transform.position);

                if (distance < bulletRadius + playerRadius)
                {
                    Debug.Log("Bullet hit the Player!");
                    Destroy(player);
                    Destroy(gameObject);
                }
            }
        }

        void CheckCollisionWithShield()
        {
            GameObject shield = GameObject.FindGameObjectWithTag("Shield");
            if (shield != null)
            {
                float shieldRadius = 0.5f;
                float distance = GetDistance(transform.position, shield.transform.position);

                // If the bullet overlaps with the shield, destroy both.
                if (distance < bulletRadius + shieldRadius)
                {
                    Debug.Log("Bullet hit the Shield!");
                    Destroy(shield);
                    Destroy(gameObject);
                }
            }
        }

        // Calculates the Euclidean distance between two points.
        private float GetDistance(Vector3 a, Vector3 b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        // Normalizes a given vector (makes its length = 1).
        private Vector2 NormalizeVector(Vector2 v)
        {
            float mag = GetVectorMagnitude(v);
            return mag == 0 ? Vector2.zero : new Vector2(v.x / mag, v.y / mag);
        }

        // Computes the magnitude (length) of a vector.
        private float GetVectorMagnitude(Vector2 v)
        {
            return (float)Math.Sqrt(v.x * v.x + v.y * v.y);
        }

        // Clamps a value between a minimum and maximum.
        private float Clamp(float value, float min, float max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

    }
}
