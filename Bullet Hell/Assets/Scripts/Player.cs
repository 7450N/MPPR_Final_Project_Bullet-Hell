using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movementInput;
    public float movementSpeed = 10f;
    private bool shooting = false;
    private Coroutine shootingCoroutine;
    public float bulletsPerSecond = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
        movementInput = movementInput.normalized;

        if (movementInput != Vector2.zero)
        {
            rb.velocity = movementInput * movementSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shooting = true;
            if (shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(ShootContinuously());
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            shooting = false;
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null;
            }
        }
    }
    private IEnumerator ShootContinuously()
    {
        while (shooting)
        {
            PlayerShooting playerShoot = FindObjectOfType<PlayerShooting>();
            playerShoot.Shoot();
            yield return new WaitForSeconds(1/bulletsPerSecond); 
        }
    }
}
