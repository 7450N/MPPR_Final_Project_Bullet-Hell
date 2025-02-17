using Janaw;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Jason

namespace Jason
{
    public class Turrent : MonoBehaviour
    {
        [Range(20f, 100f)] private float rotationSpeed; // Rotation speed of the turrent
        private int dir;
        private int attackPattern; // Attack pattern of the turrent

        public GameObject normalBulletPrefab;
        public GameObject homingBulletPrefab;
        public GameObject waveBulletPrefab;

        public int bulletCount;
        public float bulletSpeed;
        public float radius;
        public float bulletInterval;

        public float angleOffset;
        public float bulletRotationSpeed; // Degrees per shot when firing in a spiral pattern

        public float spreadAngle; // Angle between bullets in degrees when firing in a cone pattern

        public float frequency = 5f;           // Frequency of the wave pattern when firing wave bullets
        public float magnitude = 0.5f;          // Magnitude of the wave pattern when firing wave bullets

        // Start is called before the first frame update
        void Start()
        {
            rotationSpeed = Random.Range(20f, 100f); // Randomize the rotation speed of the turrent
            dir = Random.Range(0, 2) * 2 - 1; // Randomize the direction of the rotation
            attackPattern = Random.Range(0, 5); // Randomize the attack pattern of the turrent
            switch (attackPattern)
            {
                case 0:
                    bulletCount = 12;
                    bulletSpeed = 5f;
                    radius = 1f;
                    bulletInterval = 1f;
                    StartCoroutine(FireCircularPattern());
                    break;
                case 1:
                    bulletCount = 10;
                    bulletSpeed = 5f;
                    angleOffset = 0f;
                    rotationSpeed = 10f;
                    bulletInterval = 0.5f;
                    StartCoroutine(SpiralFire());
                    break;
                case 2:
                    bulletCount = 5;
                    bulletSpeed = 5f;
                    spreadAngle = 30f;
                    bulletInterval = 2f;
                    StartCoroutine(FireConePattern());
                    break;
                case 3:
                    bulletCount = 5;
                    bulletSpeed = 5f;
                    frequency = 5f;
                    magnitude = 0.5f;
                    bulletInterval = 1.5f;
                    StartCoroutine(FireWavePattern());
                    break;
                case 4:
                    bulletSpeed = 5f;
                    bulletInterval = 2.5f;
                    StartCoroutine(FireHomingBullet());
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //transform.rotation *= Quaternion.Euler(0, 0, rotationSpeed * dir * Time.deltaTime); // rotate in the z-axis
            transform.rotation *= Quaternion.Euler(0, rotationSpeed * dir * Time.deltaTime, 0); // rotate in the y-axis
        }


        private IEnumerator FireCircularPattern()
        {
            while (true)
            {
                for (int i = 0; i < bulletCount; i++)
                {
                    float angle = i * (360f / bulletCount);
                    Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
                    GameObject bullet = Instantiate(normalBulletPrefab, transform.position + direction * radius, Quaternion.identity);
                    bullet.AddComponent<BulletMovement>().SetDirection(direction, bulletSpeed);
                }
                yield return new WaitForSeconds(bulletInterval);
            }
        }


        IEnumerator SpiralFire()
        {
            while (true)
            {
                for (int i = 0; i < bulletCount; i++)
                {
                    float angle = angleOffset + i * (360f / bulletCount);
                    Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
                    GameObject bullet = Instantiate(normalBulletPrefab, transform.position, Quaternion.identity);
                    bullet.AddComponent<BulletMovement>().SetDirection(direction, bulletSpeed);

                    angleOffset += rotationSpeed; // Rotate the next batch

                }
                yield return new WaitForSeconds(bulletInterval);
            }
        }

        private IEnumerator FireConePattern()
        {
            while (true)
            {
                for (int i = 0; i < bulletCount; i++)
                {
                    float angle = -spreadAngle / 2 + (spreadAngle / (bulletCount - 1)) * i;
                    Vector3 direction = Quaternion.Euler(0, 0, angle) * transform.up;
                    GameObject bullet = Instantiate(normalBulletPrefab, transform.position, Quaternion.identity);
                    bullet.AddComponent<BulletMovement>().SetDirection(direction, bulletSpeed);
                }
                yield return new WaitForSeconds(bulletInterval);
            }
        }

        IEnumerator FireWavePattern()
        {
            while (true)
            {
                for (int i = 0; i < bulletCount; i++)
                {
                    GameObject bullet = Instantiate(waveBulletPrefab, transform.position, Quaternion.identity);
                    WaveBullet bulletScript = bullet.AddComponent<WaveBullet>();
                    bulletScript.SetParameters(transform.right, bulletSpeed, frequency, magnitude);
                }
                yield return new WaitForSeconds(bulletInterval);
            }
        }

        IEnumerator FireHomingBullet()
        {
            while (true)
                while (true)
                {
                    GameObject bullet = Instantiate(homingBulletPrefab, transform.position, Quaternion.identity);
                    bullet.AddComponent<HomingBullet>().SetSpeed(bulletSpeed);
                    yield return new WaitForSeconds(bulletInterval);
                }
        }
    }
}
