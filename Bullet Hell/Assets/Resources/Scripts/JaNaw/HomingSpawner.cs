using System.Collections;
using UnityEngine;

namespace Janaw
{
    public class HomingSpawner : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public float bulletPerSecond = 5f;
        public float waveDuration = 2f;
        public float pauseDuration = 1f;

        void Start()
        {
            StartCoroutine(SpawnBezierBullets());
        }

        IEnumerator SpawnBezierBullets()
        {
            while (true)
            {
                float waveEndTime = Time.time + waveDuration;

                while (Time.time < waveEndTime)
                {
                    // Spawn 3 bullets in different paths
                    Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    Instantiate(bulletPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    Instantiate(bulletPrefab, transform.position + new Vector3(0, -1, 0), Quaternion.identity);

                    yield return new WaitForSeconds(1/bulletPerSecond);
                }

                yield return new WaitForSeconds(pauseDuration);
            }
        }
    }
}
