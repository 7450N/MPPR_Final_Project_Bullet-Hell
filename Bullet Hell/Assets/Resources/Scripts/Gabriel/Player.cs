using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Gabriel{
    public class Player : MonoBehaviour
    {
        private Vector2 movementInput;
        public float movementSpeed = 10f;
        private bool shooting = false;
        private Coroutine shootingCoroutine;
        public float bulletsPerSecond = 10f;
        public float energyPerSecond = 20f;
        public float healRate = 0.2f;
        public float detectionRange = 0.4f;
        public int hp = 100;
        private int maxHp;
        public int energy = 100;
        private int maxEnergy;
        public Image energyBar;
        public GameObject healthBar; // Assign in Inspector
        private Image healthBarFill;
        private Image energyBarFill;
        private Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
            maxHp = hp;
            maxEnergy = energy;
            energyBarFill = energyBar.transform.GetChild(0).GetComponent<Image>();
            healthBarFill = healthBar.transform.GetChild(0).GetComponent<Image>();
            StartCoroutine(GainEnergy());
            StartCoroutine(PassiveHeal());
        }

        void Update()
        {
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");
            movementInput = movementInput.normalized;

            if (movementInput != Vector2.zero)
            {
                transform.Translate(movementInput * movementSpeed * Time.deltaTime);
                ClampPosition(); // Ensure player stays within the screen
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && energy > 0)
            {
                shooting = true;
                if (shootingCoroutine == null)
                {
                    shootingCoroutine = StartCoroutine(ShootContinuously());
                }
            }
            if (Input.GetKeyUp(KeyCode.Mouse0) || energy <= 0)
            {
                shooting = false;
                if (shootingCoroutine != null)
                {
                    StopCoroutine(shootingCoroutine);
                    shootingCoroutine = null;
                }
            }

            CheckForTaggedObjects();
            if (hp <= 0)
            {
                Destroy(healthBar);
                Destroy(gameObject);
            }
        }

        private void ClampPosition()
        {
            if (mainCamera == null) return;

            Vector3 pos = transform.position;
            Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            // Clamp the player's position within the screen boundaries
            pos.x = Mathf.Clamp(pos.x, -screenBounds.x, screenBounds.x);
            pos.y = Mathf.Clamp(pos.y, -screenBounds.y, screenBounds.y);

            transform.position = pos;
        }

        private void CheckForTaggedObjects()
        {
            GameObject[] projectiles = GameObject.FindGameObjectsWithTag("enemy projectile");
            foreach (GameObject projectile in projectiles)
            {
                if (Vector2.Distance(transform.position, projectile.transform.position) < detectionRange)
                {
                    hp -= 3;
                    Destroy(projectile);
                    return;
                }
            }
        }

        private void UpdateHealthBar()
        {
            float healthPercent = (float)hp / maxHp;
            healthBarFill.fillAmount = healthPercent;
        }

        private void UpdateEnergyBar()
        {
            float energyPercent = (float)energy / maxEnergy;
            energyBarFill.fillAmount = energyPercent;
        }
        private IEnumerator ShootContinuously()
        {
            while (shooting)
            {
                PlayerShooting playerShoot = FindObjectOfType<PlayerShooting>();
                playerShoot.Shoot();
                energy -= 1;
                UpdateEnergyBar();
                yield return new WaitForSeconds(1f / bulletsPerSecond);
            }
        }
        private IEnumerator GainEnergy()
        {
            while (true)
            {
                if (!shooting && energy < maxEnergy)
                {
                    energy += 1;
                    UpdateEnergyBar();
                    yield return new WaitForSeconds(1f / energyPerSecond);
                }
                else
                {
                    yield return null; // Prevent infinite loop
                }
            }
        }
        private IEnumerator PassiveHeal()
        {
            while (true)
            {
                if (hp < maxHp)
                {
                    hp += 1;
                    UpdateHealthBar();
                    yield return new WaitForSeconds(1f / healRate);
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
}