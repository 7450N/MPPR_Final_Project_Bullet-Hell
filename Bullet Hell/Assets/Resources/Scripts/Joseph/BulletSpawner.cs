using UnityEngine;

namespace Joseph
{
    public class BulletSpawner : MonoBehaviour
    {
        public GameObject bulletPrefab;  // Bullet prefab
        public float spawnInterval = 0.2f; // Time between bullet spawns
        public int bulletsPerWave = 10; // Number of bullets per wave
        public float bulletSpeed = 5f;  // Bullet speed
        public float waveAmplitude = 2f;  // Radius of the circular pattern (distance from center)
        public float spiralDistanceIncrease = 0.1f;  // How much the distance increases per bullet for spiral
        public bool isSpiralPattern = false;  // Flag to toggle between circular and spiral patterns
        public float waveFrequency = 1f; // Frequency of bullet movement in wave pattern

        private float timer;
        private float angleOffset = 0f;  // Used for spiral effect

        void Update()
        {
            // Count down the timer and spawn bullets when needed
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                SpawnBulletWave();
                timer = 0f;
            }
        }

        void SpawnBulletWave()
        {
            if (isSpiralPattern)
            {
                // Spawn bullets in a spiral pattern
                SpawnSpiralBullets();
            }
            else
            {
                // Spawn bullets in a circular pattern
                SpawnCircularBullets();
            }
        }

        // Spawn bullets in a circular pattern
        void SpawnCircularBullets()
        {
            for (int i = 0; i < bulletsPerWave; i++)
            {
                // Calculate the angle for each bullet in the circular pattern
                float angle = i * Mathf.PI * 2f / bulletsPerWave;  // 2 * PI for full circle

                // Determine the position of each bullet along the circle (radius is waveAmplitude)
                float x = Mathf.Cos(angle) * waveAmplitude;
                float y = Mathf.Sin(angle) * waveAmplitude;

                // Bullet movement direction: pointing outward from the center
                Vector2 direction = new Vector2(x, y).normalized * bulletSpeed;

                // Spawn the bullet at the calculated position (relative to the spawner's position)
                GameObject bullet = Instantiate(bulletPrefab, (Vector2)transform.position + new Vector2(x, y), Quaternion.identity);

                // If you want to assign custom behavior to the bullet, you can initialize here
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.InitializeBezierMovement(transform.position, direction, waveAmplitude, waveFrequency);
                }
            }
        }

        // Spawn bullets in a spiral pattern
        void SpawnSpiralBullets()
        {
            for (int i = 0; i < bulletsPerWave; i++)
            {
                // Increase the angle with each bullet spawn
                float angle = i * Mathf.PI * 2f / bulletsPerWave + angleOffset;

                // Increase the distance from the center (radius) slightly each time for the spiral effect
                float radius = waveAmplitude + i * spiralDistanceIncrease;

                // Calculate the position for the bullet on the spiral
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;

                // Bullet movement direction (pointing outward but with some rotation)
                Vector2 direction = new Vector2(x, y).normalized * bulletSpeed;

                // Spawn the bullet at the calculated position (relative to the spawner's position)
                GameObject bullet = Instantiate(bulletPrefab, (Vector2)transform.position + new Vector2(x, y), Quaternion.identity);

                // Initialize bullet with the direction and other properties
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.InitializeBezierMovement(transform.position, direction, waveAmplitude, waveFrequency);
                }
            }

            // Increase angle offset for the next wave to create the spiral effect
            angleOffset += Mathf.PI / 60f;  // Adjust the rate of spiral rotation
        }
    }

}
