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


        void Start()
        {
        }
        void Update()
        {
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");
            movementInput = movementInput.normalized;

            if (movementInput != Vector2.zero)
            {
                transform.Translate(movementInput * movementSpeed * Time.deltaTime);
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
    }
}
  