using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Gabriel
{
    public class EnemyBulletPattern3 : MonoBehaviour
    {
        public GameObject projectilePrefab; // Projectile to spawn for regular shots
        public GameObject projectilePrefab2; // Projectile for blast shots
        public Transform blastPoint; // Position from which blasts are fired
        public int blastCharge = 0; // Current blast charge
        public int maxBlastCharge = 250; // Max charge required for a blast
        public float bulletsPerSecond = 10f; // Fire rate
        private Vector3 worldPoint; // Position for random projectile spawn
        public float duration = 3f; // Duration for projectile movement
        public bool isShooting = false; // Whether the enemy is shooting
        private float currentAngle = 0; // Current angle for projectile spread
        public float spread = 0; // Spread of projectiles in blast
        private bool blasting = false; // Whether a blast is happening
        public int amountOfProjectiles; // Number of projectiles in blast
        public GameObject blastBar; // UI element showing blast charge
        private Image blastBarFill; // Image component for blast bar fill

        private void Start()
        {
            // Start shooting coroutine and get blast bar fill
            StartCoroutine(ShootContinuously());
            blastBarFill = blastBar.transform.GetChild(0).GetComponent<Image>();
        }

        public void ToggleShooting(bool enable)
        {
            // Toggle shooting state
            isShooting = enable;
        }

        public static Vector3 GetRandomScreenBorderPoint()
        {
            // Get a random point on the screen's border
            Camera cam = Camera.main;
            if (cam == null) return Vector3.zero;

            int border = Random.Range(0, 4);
            float x = 0, y = 0;

            // Randomly choose one of the four screen borders
            if (border == 0) { x = Random.Range(0f, Screen.width); y = Screen.height; }
            else if (border == 1) { x = Random.Range(0f, Screen.width); y = 0; }
            else if (border == 2) { x = 0; y = Random.Range(0f, Screen.height); }
            else if (border == 3) { x = Screen.width; y = Random.Range(0f, Screen.height); }

            // Convert to world point and return
            Vector3 worldPoint = cam.ScreenToWorldPoint(new Vector3(x, y, cam.nearClipPlane + 1f));
            worldPoint.z = 0;
            return worldPoint;
        }

        private IEnumerator EaseIn(GameObject projectile)
        {
            // Smoothly move the projectile towards the blast point
            Vector3 startPoint = projectile.transform.position;
            Vector3 endPoint = blastPoint.position;
            float timeElapsed = 0;

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float t = timeElapsed / duration;
                t = Mathf.Clamp01(t);
                t = t * t;

                if (projectile != null)
                {
                    projectile.transform.position = Vector3.Lerp(startPoint, endPoint, t);
                }

                yield return null;
            }

            if (projectile != null)
            {
                projectile.transform.position = endPoint;
                Destroy(projectile);
            }
        }

        private void Shoot()
        {
            // Spawn a regular projectile
            SpawnProjectile();
        }

        private void SpawnProjectile()
        {
            // Spawn a projectile at a random screen border point and move it smoothly
            worldPoint = GetRandomScreenBorderPoint();
            GameObject projectile = Instantiate(projectilePrefab, worldPoint, blastPoint.rotation);
            StartCoroutine(EaseIn(projectile));
        }

        private void Shoot2()
        {
            // Spawn multiple projectiles in a spread pattern
            for (int i = 0; i <= amountOfProjectiles; i++)
            {
                SpawnProjectile2(currentAngle);
                currentAngle += (360 / amountOfProjectiles); // Spread projectiles evenly
            }
            currentAngle += spread; // Add additional spread after each blast
        }

        private void SpawnProjectile2(float angle)
        {
            // Spawn a projectile for blast at a specific angle
            GameObject projectile = Instantiate(projectilePrefab2, blastPoint.position, blastPoint.rotation);
            projectile.transform.Rotate(0, 0, angle);
        }

        private IEnumerator ShootContinuously()
        {
            while (true)
            {
                // Regular shooting while not blasting
                if (isShooting && !blasting)
                {
                    Shoot();
                    blastCharge++; // Increment blast charge
                    UpdateBlastBar();
                }

                // Trigger blasting when max charge is reached
                if (blastCharge >= maxBlastCharge)
                {
                    blasting = true;
                }

                // Handle blasting
                if (blasting)
                {
                    Shoot2();
                    blastCharge--; // Decrease charge after blast
                    UpdateBlastBar();
                    if (blastCharge == 0)
                    {
                        blasting = false;
                        yield return new WaitForSeconds(1); // Cooldown after blast
                    }
                }

                // Wait for the next shot based on fire rate
                yield return new WaitForSeconds(1f / bulletsPerSecond);
            }
        }

        private void UpdateBlastBar()
        {
            // Update the blast charge bar
            float blastPercent = (float)blastCharge / maxBlastCharge;
            blastBarFill.fillAmount = blastPercent;
        }
    }
}// unrelated to the code but im listening to hatsune miku rn
