using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int enemyHp = 200;
    public int phase = 1;
    private int enemyMaxHp = 200;
    public bool enemyDead = false;
    public float detectionRange = 1;
    public GameObject infinityPoint;
    public GameObject healthBar;
    public GameObject blastBar;
    private Image healthBarFill;
    private EnemyBulletPattern1 pattern1;
    private EnemyBulletPattern2 pattern2;
    private EnemyBulletPattern3 pattern3;

    private void Start()
    {
        pattern1 = GetComponent<EnemyBulletPattern1>();
        pattern2 = GetComponent<EnemyBulletPattern2>();
        pattern3 = GetComponent<EnemyBulletPattern3>();
        enemyMaxHp = enemyHp;
        healthBarFill = healthBar.transform.GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {        
        if (enemyHp <= 0)
        {

            enemyDead = true;
            Destroy(healthBar);
            Destroy(gameObject);
        }
        else if (enemyHp <= enemyMaxHp / 3)
        {
            phase += 1;
            pattern2.ToggleShooting(false);
            Destroy(infinityPoint);
            blastBar.SetActive(true);
            pattern3.ToggleShooting(true);
        }
        else if (enemyHp <= enemyMaxHp/3 * 2)
        {
            phase += 1;
            pattern1.ToggleShooting(false);
            pattern2.ToggleShooting(true);
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
                TakeDamage(10);
                Destroy(projectile);
            }
        }
    }

    private void TakeDamage(int damage)
    {
        enemyHp -= damage;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float healthPercent = (float)enemyHp / enemyMaxHp;
        healthBarFill.fillAmount = healthPercent;
    }
}
