using System.Collections;
using UnityEngine;

public class J_Boss : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public float moveSpeed = 2f;
    public Vector2 moveAreaMin, moveAreaMax;
    private bool isMoving = false;
    private Vector2 targetPos;
    private Vector2 bossHitboxSize = new Vector2(2f, 2f);

    void Start()
    {
        currentHealth = maxHealth;
        SetBossZAxis();
        StartCoroutine(BossBehavior());
    }

    void SetBossZAxis()
    {
        // Ensure the boss stays at the same Z position (-1) to appear in front of other objects.
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(currentPosition.x, currentPosition.y, -1f);
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
        targetPos = new Vector2(
            Random.Range(moveAreaMin.x, moveAreaMax.x),  // Randomize X position within the movement area.
            Random.Range(moveAreaMin.y, moveAreaMax.y)   // Randomize Y position within the movement area.
        );

        while (Vector2.Distance(transform.position, targetPos) > 0.1f)
        {
            Vector2 moveDirection = (targetPos - (Vector2)transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            SetBossZAxis();
            yield return null;
        }

        isMoving = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            BossDefeated();
        }
    }

    void BossDefeated()
    {
        Destroy(gameObject);
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
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("PlayerBullet");

        foreach (GameObject bullet in bullets)
        {
            if (IsBulletWithinHitbox(bullet))
            {
                J_PlayerBullet bulletScript = bullet.GetComponent<J_PlayerBullet>();
                if (bulletScript != null)
                {
                    TakeDamage(bulletScript.damage);
                    Destroy(bullet);
                    break;
                }
            }
        }
    }

    bool IsBulletWithinHitbox(GameObject bullet)
    {
        Vector2 bossCenter = (Vector2)transform.position;
        Vector2 bossMin = bossCenter - bossHitboxSize / 2;
        Vector2 bossMax = bossCenter + bossHitboxSize / 2;

        Vector2 bulletPosition = bullet.transform.position;

        return bulletPosition.x >= bossMin.x && bulletPosition.x <= bossMax.x &&
               bulletPosition.y >= bossMin.y && bulletPosition.y <= bossMax.y;
    }

    private float GetDistance(Vector2 a, Vector2 b)
    {
        // Calculate the Euclidean distance between two points (Pythagorean theorem).
        float dx = a.x - b.x;
        float dy = a.y - b.y;
        return Mathf.Sqrt(dx * dx + dy * dy);  // Return the distance between the points.
    }
}
