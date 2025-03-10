using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Janaw
{
    public class Player : MonoBehaviour
    {
        private Vector2 movementInput;
        public float movementSpeed = 10f;
        private bool shooting = false;
        private Coroutine shootingCoroutine;
        public float bulletsPerSecond = 10f;
        public Transform bulletSpawn;
        public GameObject bulletPrefab;
        public bool shootTwoBullets = false;

        public int maxHealth = 100;
        public int currentHealth;
        private float detectionRange = 0.2f;


        void Start()
        {
            currentHealth = maxHealth;
        }
        void Update()
        {
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");
            movementInput = movementInput.normalized;

            if (movementInput != Vector2.zero)
            {
                Vector4 newPosition = transform.position + (Vector3)(movementInput * movementSpeed * Time.deltaTime);
                newPosition = ClampPositionToScreen(newPosition);
                transform.position = newPosition;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                shooting = true;
                if (shootingCoroutine == null)
                {
                    shootingCoroutine = StartCoroutine(ShootContinuously());
                }
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                shooting = false;
                if (shootingCoroutine != null)
                {
                    StopCoroutine(shootingCoroutine);
                    shootingCoroutine = null;
                }
            }

            CheckForBullets();
        }

        private Vector3 ClampPositionToScreen(Vector3 position)
        {
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);
            viewportPosition.x = Mathf.Clamp01(viewportPosition.x);
            viewportPosition.y = Mathf.Clamp01(viewportPosition.y);
            return Camera.main.ViewportToWorldPoint(viewportPosition);
        }
        private IEnumerator ShootContinuously()
        {
            while (shooting)
            {
                if (shootTwoBullets)
                {
                    GameObject bullet1 = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                    GameObject bullet2 = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                    bullet2.GetComponent<Bullet>().secondBullet = true;

                }
                else
                    Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                yield return new WaitForSeconds(1f / bulletsPerSecond);
            }
        }

        private void CheckForBullets()
        {
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

            foreach (GameObject bullet in bullets)
            {
                if (Vector2.Distance(transform.position, bullet.transform.position) < detectionRange)
                {
                    TakeDamage(5);
                    Destroy(bullet);
                }
            }
        }

        private void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player died");
        }
    }
}
  