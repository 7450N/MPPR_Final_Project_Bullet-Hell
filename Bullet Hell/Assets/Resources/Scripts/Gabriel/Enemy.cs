using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gabriel
{
    public class Enemy : MonoBehaviour
    {
        public int enemyHp = 200; // Enemy's current health
        private int enemyMaxHp = 200; // Enemy's maximum health
        public bool enemyDead = false; // Tracks if the enemy is dead
        public float detectionRange = 1; // Range to detect player projectiles
        public GameObject infinityPoint; // Reference to a special game object
        public GameObject healthBar; // Health bar UI
        public GameObject blastBar; // UI element for blast mode
        private Image healthBarFill; // Health bar fill component
        private EnemyBulletPattern1 pattern1; // First bullet pattern
        private EnemyBulletPattern2 pattern2; // Second bullet pattern
        private EnemyBulletPattern3 pattern3; // Third bullet pattern

        private void Start()
        {
            // Get references to bullet patterns
            pattern1 = GetComponent<EnemyBulletPattern1>();
            pattern2 = GetComponent<EnemyBulletPattern2>();
            pattern3 = GetComponent<EnemyBulletPattern3>();
            enemyMaxHp = enemyHp; // Set max HP
            healthBarFill = healthBar.transform.GetChild(0).GetComponent<Image>(); // Get health bar fill
        }

        private void Update()
        {
            if (enemyHp <= 0)
            {
                // Enemy dies, destroy objects
                enemyDead = true;
                Destroy(healthBar);
                Destroy(gameObject);
            }
            else if (enemyHp <= enemyMaxHp / 3)
            {
                // Switch to pattern3, enable blast bar, and remove infinityPoint
                pattern2.ToggleShooting(false);
                Destroy(infinityPoint);
                blastBar.SetActive(true);
                pattern3.ToggleShooting(true);
            }
            else if (enemyHp <= enemyMaxHp / 3 * 2)
            {
                // Switch from pattern1 to pattern2
                pattern1.ToggleShooting(false);
                pattern2.ToggleShooting(true);
            }

            CheckForProjectiles(); // Check if hit by projectiles
        }

        private void CheckForProjectiles()
        {
            // Find all projectiles tagged as "player projectile"
            GameObject[] projectiles = GameObject.FindGameObjectsWithTag("player projectile");

            foreach (GameObject projectile in projectiles)
            {
                // If a projectile is close, take damage and destroy it
                if (Vector2.Distance(transform.position, projectile.transform.position) < detectionRange)
                {
                    TakeDamage(10);
                    Destroy(projectile);
                }
            }
        }

        private void TakeDamage(int damage)
        {
            // Reduce enemy HP and update UI
            enemyHp -= damage;
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            // Adjust health bar fill amount based on current HP
            float healthPercent = (float)enemyHp / enemyMaxHp;
            healthBarFill.fillAmount = healthPercent;
        }
    }
}

