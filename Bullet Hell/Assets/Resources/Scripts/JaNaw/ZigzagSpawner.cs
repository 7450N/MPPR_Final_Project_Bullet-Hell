using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagSpawnerJ : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletPerSecond = 10f;
    public float waveDuration = 2f;
    public float pauseDuration = 1f;

    void Start()
    {
        StartCoroutine(SpawnZigzagBullets());
    }

    IEnumerator SpawnZigzagBullets()
    {
        while (true)
        {
            float waveEndTime = Time.time + waveDuration;
            while (Time.time < waveEndTime)
            {
                GameObject bullet1 = Instantiate(bulletPrefab, transform.position, transform.rotation);
                GameObject bullet2 = Instantiate(bulletPrefab, transform.position, transform.rotation);
                bullet2.GetComponent<ZigZagBulletJ>().secondBullet = true;
                yield return new WaitForSeconds(1f / bulletPerSecond);
            }

         yield return new WaitForSeconds(pauseDuration);
        }
    }
}
