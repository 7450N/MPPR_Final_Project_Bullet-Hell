using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletJ : MonoBehaviour
{
    public float speed = 10f;
    public float zigzagAmplitude = 1f; // How far left and right the bullet moves
    public float zigzagFrequency = 5f; // How fast the zigzag happens
    public bool secondBullet = false;
    private float startTime;
    private Vector2 startPosition;
    public float radiusGrowth = 0.5f; // How much the radius increases over time

    void Start()
    {
        startTime = Time.time;
        startPosition = transform.position;
    }

    void Update()
    {
        float timeElapsed = Time.time - startTime;
        float radius = radiusGrowth * timeElapsed;

        // Move up along the y-axis
        float newX = startPosition.x + speed * timeElapsed;

        // Zig-zag movement along the x-axis
        float newY;
        if (secondBullet)
        {   
            newY = startPosition.y + Mathf.Sin(timeElapsed * zigzagFrequency + Mathf.PI) * zigzagAmplitude * radius;
        }
        else
        {
            newY = startPosition.y + Mathf.Sin(timeElapsed * zigzagFrequency) * zigzagAmplitude * radius;
        }

        transform.position = new Vector2(newX, newY);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
