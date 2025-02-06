using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPattern2 : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    private Coroutine shootingCoroutine;
    public float bulletsPerSecond = 10f;
    private Enemy enemy;
    private Transform player;
    public float duration = 3f;

    private void Start()
    {
        enemy = GetComponent<Enemy>(); // Ensure enemy is assigned before using it
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Find the player by tag

        if (enemy == null)
        {
            Debug.LogError("EnemyBulletPattern2: No Enemy component found on " + gameObject.name);
            return;
        }

        if (player == null)
        {
            Debug.LogError("EnemyBulletPattern2: No GameObject with tag 'Player' found!");
            return;
        }

        shootingCoroutine = StartCoroutine(ShootContinuously());
    }

    private void Update()
    {
        if (enemy != null && enemy.enemyDead && shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    public void Shoot()
    {
        SpawnProjectile();
    }

    private void SpawnProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        StartCoroutine(MoveAlongBezierCurve(projectile));
    }

    private IEnumerator MoveAlongBezierCurve(GameObject projectile)
    {
        if (player == null) yield break;

        Vector3 startPoint = projectile.transform.position;
        Vector3 endPoint = player.position;
        Vector3 controlPoint = GetRandomScreenPoint();

        float timeElapsed = 0;


        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;

            // Calculate Bezier position
            Vector3 bezierPosition = Mathf.Pow(1 - t, 2) * startPoint +
                                     2 * (1 - t) * t * controlPoint +
                                     Mathf.Pow(t, 2) * endPoint;

            
            if (projectile != null)
            {
                projectile.transform.position = bezierPosition;
            }
            yield return null;
        }

        if (projectile != null)
        {
            projectile.transform.position = endPoint;
            Destroy(projectile);
        }

    }

    private Vector3 GetRandomScreenPoint()
    {
        Camera cam = Camera.main;
        if (cam == null) return Vector3.zero;

        // Generate a random point on the screen
        Vector3 randomScreenPos = new Vector3(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height), cam.nearClipPlane + 5f);

        // Convert screen point to world point
        Vector3 worldPoint = cam.ScreenToWorldPoint(randomScreenPos);
        return worldPoint;
    }

    private IEnumerator ShootContinuously()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(1f / bulletsPerSecond);
        }
    }
}
