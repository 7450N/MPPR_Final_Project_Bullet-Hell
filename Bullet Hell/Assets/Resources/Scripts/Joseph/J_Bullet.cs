using UnityEngine;
using System;

public class J_Bullet : MonoBehaviour
{
    private Vector2 startPosition;
    private Vector2 direction;
    private float speed;
    private float amplitude;
    private float frequency;
    private float timeElapsed = 0f;
    private float bulletRadius = 0.3f;

    public void InitializeBezierMovement(Vector2 start, Vector2 dir, float amp, float freq)
    {
        startPosition = start;
        direction = NormalizeVector(dir);  // Normalize the direction vector.
        speed = GetVectorMagnitude(dir);  // Get the magnitude of the direction vector.
        amplitude = amp;
        frequency = freq;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Apply a wave offset to create the curved movement.
        float waveOffset = CustomSin(timeElapsed * frequency) * amplitude;

        // Perpendicular vector to the direction.
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);

        // Calculate the new position using the wave offset.
        Vector2 curvedPosition = startPosition + direction * (speed * timeElapsed) + perpendicular * waveOffset;

        transform.position = curvedPosition;

        CheckCollisionWithPlayer();

        if (timeElapsed > 6f)
        {
            Destroy(gameObject);
        }
    }

    void CheckCollisionWithPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Calculate the distance between the bullet and the player.
            float distance = GetDistance(transform.position, player.transform.position);

            if (distance < bulletRadius + 0.2f)  // Check if the distance is less than the combined radii.
            {
                Debug.Log("Bullet hit the Player!");
                Destroy(player);
                Destroy(gameObject);
            }
        }
    }

    // Euclidean distance between two points.
    private float GetDistance(Vector3 a, Vector3 b)
    {
        float dx = a.x - b.x;
        float dy = a.y - b.y;
        return (float)Math.Sqrt(dx * dx + dy * dy);  // Pythagorean theorem.
    }

    private Vector2 NormalizeVector(Vector2 v)
    {
        float mag = GetVectorMagnitude(v);
        return mag == 0 ? Vector2.zero : new Vector2(v.x / mag, v.y / mag);  // Normalize the vector.
    }

    private float GetVectorMagnitude(Vector2 v)
    {
        return (float)Math.Sqrt(v.x * v.x + v.y * v.y);  // Magnitude of the vector.
    }

    // Custom sine approximation (Taylor series expansion).
    private float CustomSin(float x)
    {
        float x2 = x * x;
        return x - (x2 * x) / 6 + (x2 * x2 * x) / 120 - (x2 * x2 * x2 * x) / 5040;
    }
}
