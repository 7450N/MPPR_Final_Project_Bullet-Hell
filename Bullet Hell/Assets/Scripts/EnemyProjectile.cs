using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime; // Projectile movement
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
