using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Janaw
{
    public class Boss : MonoBehaviour
    {
        public float moveSpeed = 0.5f;
        public float areaSize = 2f; // Defines the range in which the boss moves H
        private Vector3 initialPosition;
        private Vector3 targetPosition;

        public int maxHealth = 150;
        public int currentHealth;
        public float detectionRange = 0.2f;
        void Start()
        {
            initialPosition = transform.position;
            SetNewTargetPosition();
            currentHealth = maxHealth;
        }

        void Update()
        {
            Move();
            CheckForPlayerBullets();
        }

        void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                SetNewTargetPosition();
            }


        }
        void SetNewTargetPosition()
        {
            float randomX = Random.Range(initialPosition.x - areaSize, initialPosition.x + areaSize);
            float randomY = Random.Range(initialPosition.y - areaSize, initialPosition.y + areaSize);
            targetPosition = new Vector3(randomX, randomY, transform.position.z);
        }

        private void CheckForPlayerBullets()
        {
            GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
            
            foreach (GameObject bullet in playerBullets)
            {
                if (Vector2.Distance(transform.position, bullet.transform.position) < detectionRange)
                {
                    TakeDamage(10);
                    Destroy(bullet);
                }
            }
        }


        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Boss died");
            Destroy(gameObject);
        }
    }
}