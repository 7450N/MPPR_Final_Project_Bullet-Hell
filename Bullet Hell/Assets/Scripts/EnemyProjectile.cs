using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;
    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime; //projectile movement
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit player");
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("border"))
        {
            Destroy(gameObject);
        }

    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
