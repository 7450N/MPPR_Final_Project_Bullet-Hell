using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyHp = 200;
    public bool enemyDead = false;
    public float detectionRange = 1;

    private void Update()
    {
        if (enemyHp <= 0)
        {
            enemyDead = true;
            Destroy(gameObject);
        }

        CheckForProjectiles();
    }

    private void CheckForProjectiles()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("player projectile");

        foreach (GameObject projectile in projectiles)
        {
            if (Vector2.Distance(transform.position, projectile.transform.position) < detectionRange)
            {
                enemyHp -= 10;
                Debug.Log("enemyhp:" + enemyHp);
                Destroy(projectile);
                return;
            }
        }
    }
}
