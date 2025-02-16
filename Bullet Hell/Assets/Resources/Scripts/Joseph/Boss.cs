using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public float moveSpeed = 2f;
    public Vector2 moveAreaMin, moveAreaMax;
    private bool isMoving = false;
    private Vector2 targetPos;

    private Vector2 bossHitboxSize = new Vector2(2f, 2f); // Boss hitbox size (2x2)

    void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(BossBehavior());
    }

    IEnumerator BossBehavior()
    {
        while (currentHealth > 0)
        {
            if (!isMoving)
            {
                StartCoroutine(MoveToRandomPosition());
            }

            yield return new WaitForSeconds(1f);
        }

        BossDefeated();
    }

    IEnumerator MoveToRandomPosition()
    {
        isMoving = true;

        // Generate a random target position within the movement area
        targetPos = new Vector2(
            Random.Range(moveAreaMin.x, moveAreaMax.x),
            Random.Range(moveAreaMin.y, moveAreaMax.y)
        );

        // Move towards the target position
        while (Vector2.Distance(transform.position, targetPos) > 0.1f)
        {
            Vector2 moveDirection = (targetPos - (Vector2)transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Boss HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            BossDefeated();
        }
    }

    void BossDefeated()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject); // Destroy the Boss
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    void Update()
    {
        CheckBulletCollision();
    }

    void CheckBulletCollision()
    {
        // Get all objects with "PlayerBullet" tag
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("PlayerBullet");

        foreach (GameObject bullet in bullets)
        {
            // Check if the bullet is within the boss's 2x2 hitbox
            if (IsBulletWithinHitbox(bullet))
            {
                // Assuming damage value is stored in the bullet's script
                PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();
                if (bulletScript != null)
                {
                    TakeDamage(bulletScript.damage);
                    Destroy(bullet); // Destroy the bullet after it hits
                    break; // Exit the loop after taking damage
                }
            }
        }
    }

    // Check if the bullet is within the boss's 2x2 hitbox
    bool IsBulletWithinHitbox(GameObject bullet)
    {
        // Calculate the bounding box of the boss
        Vector2 bossCenter = (Vector2)transform.position;
        Vector2 bossMin = bossCenter - bossHitboxSize / 2;
        Vector2 bossMax = bossCenter + bossHitboxSize / 2;

        // Get the bullet's position
        Vector2 bulletPosition = bullet.transform.position;

        // Check if the bullet is within the boss's hitbox (bounding box check)
        return bulletPosition.x >= bossMin.x && bulletPosition.x <= bossMax.x &&
               bulletPosition.y >= bossMin.y && bulletPosition.y <= bossMax.y;
    }

    // Simple distance calculation function (for reference)
    private float GetDistance(Vector2 a, Vector2 b)
    {
        float dx = a.x - b.x;
        float dy = a.y - b.y;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }
}
