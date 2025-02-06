using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 movementInput;
    public float movementSpeed = 10f;
    private bool shooting = false;
    private Coroutine shootingCoroutine;
    public float bulletsPerSecond = 10f;
    public float detectionRange = 0.4f;

    void Start()
    {
    }
    void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
        movementInput = movementInput.normalized;

        if (movementInput != Vector2.zero)
        {
            transform.Translate(movementInput * movementSpeed * Time.deltaTime);
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
        CheckForTaggedObjects();
    }
    private void CheckForTaggedObjects()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("enemy projectile");
        foreach (GameObject projectile in projectiles)
        {
            if (Vector2.Distance(transform.position, projectile.transform.position) < detectionRange)
            {
                Debug.Log("hit player");
                Destroy(projectile);
                return;
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
