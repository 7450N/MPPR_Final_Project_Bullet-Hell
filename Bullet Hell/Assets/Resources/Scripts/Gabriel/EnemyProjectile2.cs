using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
namespace Gabriel{
    public class EnemyProjectile2 : MonoBehaviour
    {
        public float speed = 10f;
        public GameObject projectilePrefab;
        public int amountOfProjectiles;
        private float currentAngle = 0;

        private void Start()
        {
            currentAngle = Random.Range(1, 360 / amountOfProjectiles);
        }
        private void Update()
        {
            transform.position += transform.up * speed * Time.deltaTime; //projectile movement
        }
        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
        public void Split()
        {
            for (int i = 0; i <= amountOfProjectiles; i++)
            {
                SpawnProjectile(currentAngle);
                currentAngle += (360 / amountOfProjectiles);
            }
        }

        private void SpawnProjectile(float angle)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.transform.Rotate(0, 0, angle);
        }

        private void OnDestroy()
        {
            Split();
        }
    }
}
