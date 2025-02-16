using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoBulletJ : MonoBehaviour
{
    public float speed = 5f; // Vertical speed
    public float rotationSpeed = 3f; // How fast it rotates
    public float radiusGrowth = 0.5f; // How much the radius increases over time

    private float startTime;
    private Vector2 startPosition;

    void Start()
    {
        startTime = Time.time;
        startPosition = transform.position;
    }

    void Update()
    {
        float timeElapsed = Time.time - startTime;

        // Increasing radius over time to create a tornado widening effect
        float radius = radiusGrowth * timeElapsed;

        // Circular motion
        float newX = startPosition.x - speed * timeElapsed;
        float newY = startPosition.y + Mathf.Cos(timeElapsed * rotationSpeed) * radius;

        transform.position = new Vector2(newX, newY);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
