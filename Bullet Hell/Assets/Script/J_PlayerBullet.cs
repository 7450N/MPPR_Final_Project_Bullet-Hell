using UnityEngine;

public class J_PlayerBullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;

    private Vector2 direction;

    private const float bossRadius = 0.5f;
    private float timeAlive = 0f;

    public void Initialize(Vector2 startPosition, Vector2 direction)
    {
        this.transform.position = startPosition;
        this.direction = direction;
    }

    void Update()
    {
        // Move the bullet in the given direction.
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // Track how long the bullet has existed.
        timeAlive += Time.deltaTime;

        // Destroy the bullet if it has existed longer than 6 seconds.
        if (timeAlive > 6f)
        {
            Destroy(gameObject);
        }
    }

    public bool CheckCollisionWithBoss(Vector2 bossPosition)
    {
        // Calculate the distance between the bullet and the boss.
        float distanceToBoss = GetDistance((Vector2)transform.position, bossPosition);

        // Check if the bullet is within the boss's hit box.
        return distanceToBoss < bossRadius;
    }

    public void DealDamageToBoss()
    {
        Debug.Log("Bullet hit the boss! Dealt " + damage + " damage.");
    }

    private float GetDistance(Vector2 a, Vector2 b)
    {
        // Calculate the difference in x and y.
        float dx = a.x - b.x;
        float dy = a.y - b.y;

        // Use the Pythagorean theorem to find the distance.
        return Mathf.Sqrt(dx * dx + dy * dy);
    }
}
