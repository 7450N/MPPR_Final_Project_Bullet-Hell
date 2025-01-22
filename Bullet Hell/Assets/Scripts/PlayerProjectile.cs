using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 10f;
    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime; //projectile movement
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            Debug.Log("hit enemy");
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("border"))
        {
            Destroy(gameObject);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

