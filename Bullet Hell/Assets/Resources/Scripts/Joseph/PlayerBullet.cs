using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 10f;  // Bullet speed
    public int damage = 10;    // Bullet damage

    private Vector2 direction; // Direction of bullet's movement

    private const float bossRadius = 0.5f;  // Boss collision radius (adjust as needed)
    private float timeAlive = 0f;  // Time elapsed since the bullet was created

    // Initialize the bullet with position and direction
    public void Initialize(Vector2 startPosition, Vector2 direction)
    {
        this.transform.position = startPosition;
        this.direction = direction;
    }

    void Update()
    {
        // Move the bullet
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // Increment the time the bullet has been alive
        timeAlive += Time.deltaTime;

        // Destroy the bullet if it has been alive for more than 5 seconds
        if (timeAlive > 6f)
        {
            Destroy(gameObject);
        }
    }

    // Check for collision with the boss
    public bool CheckCollisionWithBoss(Vector2 bossPosition)
    {
        float distanceToBoss = GetDistance((Vector2)transform.position, bossPosition);
        return distanceToBoss < bossRadius;
    }

    // Deal damage to the boss (you would likely do this in the boss script)
    public void DealDamageToBoss()
    {
        Debug.Log("Bullet hit the boss! Dealt " + damage + " damage.");
    }

    // Calculate distance between two points
    private float GetDistance(Vector2 a, Vector2 b)
    {
        float dx = a.x - b.x;
        float dy = a.y - b.y;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }
}
