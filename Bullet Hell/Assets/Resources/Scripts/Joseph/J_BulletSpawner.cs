using UnityEngine;

public class J_BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnInterval = 0.2f;
    public int bulletsPerWave = 10;
    public float bulletSpeed = 5f;
    public float waveAmplitude = 2f;
    public float spiralDistanceIncrease = 0.1f;
    public bool isSpiralPattern = false;
    public float waveFrequency = 1f;

    private float timer;
    private float angleOffset = 0f;

    void Update()
    {
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
            SpawnSpiralBullets();
        }
        else
        {
            SpawnCircularBullets();
        }
    }

    void SpawnCircularBullets()
    {
        for (int i = 0; i < bulletsPerWave; i++)
        {
            float angle = i * Mathf.PI * 2f / bulletsPerWave;

            // Calculate the x and y positions using polar coordinates.
            float x = Mathf.Cos(angle) * waveAmplitude;  // x = radius * cos.
            float y = Mathf.Sin(angle) * waveAmplitude;  // y = radius * sin.

            // Normalize the direction vector and scale it by bulletSpeed.
            Vector2 direction = new Vector2(x, y).normalized * bulletSpeed;

            GameObject bullet = Instantiate(bulletPrefab, (Vector2)transform.position + new Vector2(x, y), Quaternion.identity);

            J_Bullet bulletScript = bullet.GetComponent<J_Bullet>();
            if (bulletScript != null)
            {
                bulletScript.InitializeBezierMovement(transform.position, direction, waveAmplitude, waveFrequency);
            }
        }
    }

    void SpawnSpiralBullets()
    {
        for (int i = 0; i < bulletsPerWave; i++)
        {
            float angle = i * Mathf.PI * 2f / bulletsPerWave + angleOffset;

            // Calculate the radius, which increases as the loop progresses.
            float radius = waveAmplitude + i * spiralDistanceIncrease;

            // Calculate the x and y positions based on the current angle and radius.
            float x = Mathf.Cos(angle) * radius;  // x = radius * cos.
            float y = Mathf.Sin(angle) * radius;  // y = radius * sin.

            // Normalize the direction vector and scale it by bulletSpeed.
            Vector2 direction = new Vector2(x, y).normalized * bulletSpeed;

            GameObject bullet = Instantiate(bulletPrefab, (Vector2)transform.position + new Vector2(x, y), Quaternion.identity);

            J_Bullet bulletScript = bullet.GetComponent<J_Bullet>();
            if (bulletScript != null)
            {
                bulletScript.InitializeBezierMovement(transform.position, direction, waveAmplitude, waveFrequency);
            }
        }

        // Gradually increase the angle offset to continue the spiral effect over time.
        angleOffset += Mathf.PI / 60f;  // This rotates the bullets in a spiral.
    }
}
