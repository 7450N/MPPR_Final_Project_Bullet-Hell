using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gabriel{
    public class EnemyBulletPattern2 : MonoBehaviour
    {
        public GameObject projectilePrefab;
        public Transform firePoint;
        public float bulletsPerSecond = 10f;
        private Transform player;
        public float duration = 3f;
        public bool isShooting = false;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            StartCoroutine(ShootContinuously());
        }


        public void ToggleShooting(bool enable)
        {
            isShooting = enable;
        }

        private void Shoot()
        {
            SpawnProjectile();
        }

        private void SpawnProjectile()
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            StartCoroutine(MoveAlongBezierCurve(projectile));
        }

        private IEnumerator MoveAlongBezierCurve(GameObject projectile)
        {
            Vector3 startPoint = projectile.transform.position;
            Vector3 endPoint = player.position;
            Vector3 controlPoint = GetRandomScreenPoint();

            float timeElapsed = 0;

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float t = timeElapsed / duration;

                Vector3 bezierPosition = Mathf.Pow(1 - t, 2) * startPoint +
                                         2 * (1 - t) * t * controlPoint +
                                         Mathf.Pow(t, 2) * endPoint;

                if (projectile != null)
                {
                    projectile.transform.position = bezierPosition;
                }
                yield return null;
            }

            if (projectile != null)
            {
                projectile.transform.position = endPoint;
                Destroy(projectile);
            }
        }

        private Vector3 GetRandomScreenPoint()
        {
            Camera cam = Camera.main;
            if (cam == null) return Vector3.zero;

            Vector3 randomScreenPos = new Vector3(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height), cam.nearClipPlane + 5f);
            Vector3 worldPoint = cam.ScreenToWorldPoint(randomScreenPos);
            return worldPoint;
        }

        private IEnumerator ShootContinuously()
        {
            while (true)
            {
                if (isShooting)
                {
                    Shoot();
                }

                yield return new WaitForSeconds(1f / bulletsPerSecond);
            }
        }
    }
}