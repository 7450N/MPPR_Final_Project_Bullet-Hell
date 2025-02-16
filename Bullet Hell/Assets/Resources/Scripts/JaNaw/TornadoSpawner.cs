using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSpawnerJ : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int bulletCount = 10;
    public float bulletPerSecond = 10f;
    public float waveDuration = 2f;
    public float pauseDuration = 2f;

    void Start()
    {
        StartCoroutine(SpawnTornadoBullets());
    }

    IEnumerator SpawnTornadoBullets()
    {
        while (true)
        {
            float waveEndTime = Time.time + waveDuration;
            while (Time.time < waveEndTime)
            {
                for (int i = 0; i < bulletCount; i++)
                {
                    float angle = i * (360f / bulletCount); // Distribute bullets in a circle
                    float radians = angle * Mathf.Deg2Rad;

                    Vector2 spawnPos = new Vector2(
                        transform.position.x + Mathf.Cos(radians) * 0.1f,
                        transform.position.y + Mathf.Sin(radians) * 0.3f
                    );

                    GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
                    bullet.GetComponent<TornadoBulletJ>().rotationSpeed = 2f + i * 0.1f; // Slightly different speeds for variety
                }

                yield return new WaitForSeconds(1f / bulletPerSecond);
            }
            yield return new WaitForSeconds(pauseDuration);
        }
    }
}
