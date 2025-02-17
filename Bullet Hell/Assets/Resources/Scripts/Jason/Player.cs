using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPPR;
using System.Diagnostics.Tracing;

// written by Jason

namespace Jason
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float xMinRange = -17f; // Minimum X value for the player to move
        [SerializeField] private float xMaxRange = 17f; // Maximum X value for the player to move
        [SerializeField] private float zMinRange = -9f; // Minimum Z value for the player to move
        [SerializeField] private float zMaxRange = 9f; // Maximum Z value for the player to move
        [SerializeField] private float speed = 5f; // Speed of the player
        [SerializeField] private GameObject bulletPrefab;
        private float playerRadius;

        public Transform firePoint;
        public float bulletSpeed;

        public float fireRate = 0.2f; // rate of fire in seconds
        private float nextFireTime = 0f;

        float xInput;
        float zInput;

        public float maxHealth = 100f;
        private float currentHealth;

        private Renderer renderer;

        private Color healthyColor = Color.green;
        private Color warningColor = Color.yellow;
        private Color dangerColor = Color.red;

        // Start is called before the first frame update
        void Start()
        {
            PlayerConstants.XMaxRange = xMaxRange;           
            PlayerConstants.XMinRange = xMinRange;
            PlayerConstants.ZMaxRange = zMaxRange;
            PlayerConstants.ZMinRange = zMinRange;
            currentHealth = maxHealth;
            renderer = GetComponent<Renderer>(); // Get the Renderer component
            UpdateColor(); // Set initial color
            playerRadius = GetComponent<MeshRenderer>().bounds.extents.x;
        }

        // Update is called once per frame
        void Update()
        {
            GetInput();
            Move();
        }


        private void GetInput()
        {
            xInput = Input.GetAxis("Horizontal");           
            zInput = Input.GetAxis("Vertical");
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime) // Hold left click to shoot
            {
                Shoot();
                nextFireTime = Time.time + fireRate; // Set next allowed fire time
            }
        }


        private void Move()
        {
            Vector3 moveDir = new Vector3(xInput, 0, zInput);

            if (moveDir != Vector3.zero) moveDir.Normalize();      //Normalize the vector to get consistent speed when moving diagonally
            
            transform.Translation(moveDir * speed * Time.deltaTime, Space.Self);


            // to make the player not to go out of bound
            Vector3 clampedPos = transform.position;
            clampedPos.x = clampedPos.x.Clamp(xMinRange, xMaxRange);
            clampedPos.z = clampedPos.z.Clamp(zMinRange, zMaxRange);
            transform.position = clampedPos;
        }

        void Shoot()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.y = firePoint.position.y; // Keep it in 2D space

            Vector3 shootDirection = (mousePosition - firePoint.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();

            if (bulletScript != null)
            {
                bulletScript.SetDirection(shootDirection);
            }

            // Rotate bullet towards the direction of travel
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }


        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            currentHealth = currentHealth.Clamp( 0, maxHealth); // Ensure health doesn't go negative

            UpdateColor(); // Change color based on health

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }

        void UpdateColor()
        {
            float healthPercent = currentHealth / maxHealth;

            if (healthPercent > 0.5f)
            {
                renderer.material.color = CustomMethod.Lerp(warningColor, healthyColor, (healthPercent - 0.5f) * 2);
            }
            else
            {
                renderer.material.color = CustomMethod.Lerp(dangerColor, warningColor, healthPercent * 2);
            }
        }

        public float GetRadius()
        {
            return playerRadius;
        }
    }

}
