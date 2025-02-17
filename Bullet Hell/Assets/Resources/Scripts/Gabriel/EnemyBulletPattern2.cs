using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gabriel
{
    public class EnemyBulletPattern2 : MonoBehaviour
    {
        public GameObject projectilePrefab; // Projectile to spawn
        public Transform firePoint; // Firing position
        public float bulletsPerSecond = 10f; // Fire rate
        private Transform player; // Reference to the player
        public float duration = 3f; // Time for projectile to follow curve
        public bool isShooting = false; // Controls shooting state

        private void Start()
        {
            // Find the player in the scene
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            // Start shooting coroutine
            StartCoroutine(ShootContinuously());
        }

        public void ToggleShooting(bool enable)
        {
            // Enable or disable shooting
            isShooting = enable;
        }

        private void Shoot()
        {
            // Spawn a projectile
            SpawnProjectile();
        }

        private void SpawnProjectile()
        {
            // Instantiate a projectile and start moving it along a curve
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            StartCoroutine(MoveAlongBezierCurve(projectile));
        }

        private IEnumerator MoveAlongBezierCurve(GameObject projectile)
        {
            Vector3 startPoint = projectile.transform.position; // Start position
            Vector3 endPoint = player.position; // Target position (player)
            Vector3 controlPoint = GetRandomScreenPoint(); // Random curve point

            float timeElapsed = 0;

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float t = timeElapsed / duration;

                // Bezier curve calculation
                Vector3 bezierPosition = Mathf.Pow(1 - t, 2) * startPoint +
                                         2 * (1 - t) * t * controlPoint +
                                         Mathf.Pow(t, 2) * endPoint;

                if (projectile != null)
                {
                    projectile.transform.position = bezierPosition;
                }
                yield return null;
            }

            // Ensure projectile reaches the endpoint and then destroy it
            if (projectile != null)
            {
                projectile.transform.position = endPoint;
                Destroy(projectile);
            }
        }

        private Vector3 GetRandomScreenPoint()
        {
            // Get a random point on the screen and convert it to world space
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

                // Wait based on fire rate
                yield return new WaitForSeconds(1f / bulletsPerSecond);
            }
        }
    }
}
