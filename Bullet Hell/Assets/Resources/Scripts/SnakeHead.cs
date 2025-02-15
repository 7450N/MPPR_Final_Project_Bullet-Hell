using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public float speed = 5f; // Speed of movement
    public float directionChangeInterval = 2f; // Time interval for changing direction
    private Vector2 currentDirection;

    void Start()
    {
        // Set the initial direction
        SetRandomDirection();

        // Change direction periodically
        InvokeRepeating("SetRandomDirection", 0f, directionChangeInterval);
    }

    void Update()
    {
        // Continuously move in the current direction
        transform.Translate(currentDirection * speed * Time.deltaTime);
    }

    // Set a random direction using an angle
    void SetRandomDirection()
    {
        // Generate a random angle between 0 and 360 degrees
        float randomAngle = Random.Range(0f, 360f);

        // Convert the angle to a direction vector
        currentDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
    }
}
