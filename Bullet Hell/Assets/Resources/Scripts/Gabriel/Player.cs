using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gabriel
{
    public class Player : MonoBehaviour
    {
        private Vector2 movementInput; // Input for movement (horizontal and vertical)
        public float movementSpeed = 10f; // Speed of player movement
        private bool shooting = false; // Is the player shooting?
        private Coroutine shootingCoroutine; // Coroutine for continuous shooting
        public float bulletsPerSecond = 10f; // Bullets per second while shooting
        public float energyPerSecond = 20f; // Rate at which energy is gained
        public float healRate = 0.2f; // Rate at which health is healed over time
        public float detectionRange = 0.4f; // Range for detecting incoming projectiles
        public int hp = 100; // Current health points
        private int maxHp; // Maximum health points
        public int energy = 100; // Current energy
        private int maxEnergy; // Maximum energy
        public Image energyBar; // UI element for energy bar
        public GameObject healthBar; // UI element for health bar
        private Image healthBarFill; // UI image for the health fill
        private Image energyBarFill; // UI image for the energy fill
        private Camera mainCamera; // Reference to the main camera

        void Start()
        {
            mainCamera = Camera.main;
            maxHp = hp; // Set max health
            maxEnergy = energy; // Set max energy
            energyBarFill = energyBar.transform.GetChild(0).GetComponent<Image>();
            healthBarFill = healthBar.transform.GetChild(0).GetComponent<Image>();
            StartCoroutine(GainEnergy()); // Start regenerating energy
            StartCoroutine(PassiveHeal()); // Start passive healing
        }

        void Update()
        {
            // Handle movement input and apply it to the player
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");
            movementInput = movementInput.normalized;

            if (movementInput != Vector2.zero)
            {
                transform.Translate(movementInput * movementSpeed * Time.deltaTime);
                ClampPosition(); // Ensure player stays within the screen
            }

            // Handle shooting input and energy consumption
            if (Input.GetKeyDown(KeyCode.Mouse0) && energy > 0)
            {
                shooting = true;
                if (shootingCoroutine == null)
                {
                    shootingCoroutine = StartCoroutine(ShootContinuously()); // Start shooting coroutine
                }
            }
            if (Input.GetKeyUp(KeyCode.Mouse0) || energy <= 0)
            {
                shooting = false;
                if (shootingCoroutine != null)
                {
                    StopCoroutine(shootingCoroutine); // Stop shooting when released
                    shootingCoroutine = null;
                }
            }

            CheckForTaggedObjects(); // Check for collisions with enemy projectiles
            if (hp <= 0)
            {
                Destroy(healthBar); // Destroy health bar when player dies
                Destroy(gameObject); // Destroy the player object
            }
        }

        private void ClampPosition()
        {
            // Ensure the player stays within the screen boundaries
            if (mainCamera == null) return;

            Vector3 pos = transform.position;
            Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            pos.x = Mathf.Clamp(pos.x, -screenBounds.x, screenBounds.x); // Clamp X position
            pos.y = Mathf.Clamp(pos.y, -screenBounds.y, screenBounds.y); // Clamp Y position

            transform.position = pos;
        }

        private void CheckForTaggedObjects()
        {
            // Check for enemy projectiles within detection range
            GameObject[] projectiles = GameObject.FindGameObjectsWithTag("enemy projectile");
            foreach (GameObject projectile in projectiles)
            {
                if (Vector2.Distance(transform.position, projectile.transform.position) < detectionRange)
                {
                    hp -= 3; // Take damage
                    Destroy(projectile); // Destroy the projectile
                    return;
                }
            }
        }

        private void UpdateHealthBar()
        {
            // Update health bar UI based on current health
            float healthPercent = (float)hp / maxHp;
            healthBarFill.fillAmount = healthPercent;
        }

        private void UpdateEnergyBar()
        {
            // Update energy bar UI based on current energy
            float energyPercent = (float)energy / maxEnergy;
            energyBarFill.fillAmount = energyPercent;
        }

        private IEnumerator ShootContinuously()
        {
            // Coroutine to handle continuous shooting
            while (shooting)
            {
                PlayerShooting playerShoot = FindObjectOfType<PlayerShooting>();
                playerShoot.Shoot(); // Perform shooting action
                energy -= 1; // Decrease energy
                UpdateEnergyBar(); // Update energy bar UI
                yield return new WaitForSeconds(1f / bulletsPerSecond); // Wait based on shooting rate
            }
        }

        private IEnumerator GainEnergy()
        {
            // Coroutine to gradually regenerate energy
            while (true)
            {
                if (!shooting && energy < maxEnergy)
                {
                    energy += 1; // Regenerate energy
                    UpdateEnergyBar(); // Update energy bar UI
                    yield return new WaitForSeconds(1f / energyPerSecond); // Wait before next regen
                }
                else
                {
                    yield return null; // Prevent infinite loop
                }
            }
        }

        private IEnumerator PassiveHeal()
        {
            // Coroutine to gradually heal the player over time
            while (true)
            {
                if (hp < maxHp)
                {
                    hp += 1; // Heal the player
                    UpdateHealthBar(); // Update health bar UI
                    yield return new WaitForSeconds(1f / healRate); // Wait before next healing
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
}
