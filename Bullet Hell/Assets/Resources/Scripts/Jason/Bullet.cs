/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPPR;
using System.Runtime.CompilerServices;

namespace Jason
{
    public class Bullet : MonoBehaviour
    {   
        private enum AttackType
        {
            Player,
            Enemy
        }

        [SerializeField] private float speed = 10f; // Speed of the bullet
        [SerializeField] private float lifeTime = 2f; // Time before the bullet is destroyed
        private float lifeTimer; // Timer for the bullet's life
        // Start is called before the first frame update

        void Start()
        {
           float attackPattern = Random.Range(0, AttackType.length);

        }
        // Update is called once per frame
        void Update()
        {
            Move();
            CheckLifeTime();
        }


        private void Move()
        {
            transform.Translation(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }


        private void CheckLifeTime()
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
*/
