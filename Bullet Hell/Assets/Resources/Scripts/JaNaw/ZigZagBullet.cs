using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Janaw
{
    public class ZigZagBullet : MonoBehaviour
    {
        public Transform player;
        public float speed = 10f;
        public float zigzagAmplitude = 1f; // How far left and right the bullet moves
        public float zigzagFrequency = 5f; // How fast the zigzag happens
        public bool secondBullet = false;

        private float startTime;
        private Vector2 startPosition;
        private Vector2 direction;
        public float radiusGrowth = 0.5f;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

            startTime = Time.time;
            startPosition = transform.position;

            if (player != null)
            {
                direction = (player.position - transform.position).normalized;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (player != null)
            {
                float timeElapsed = Time.time - startTime;
                float radius = radiusGrowth * timeElapsed;

                // Move along the direction vector
                Vector2 moveDirection = direction * speed * timeElapsed;

                // Zig-zag movement perpendicular to the direction vector
                Vector2 perpendicularDirection = new Vector2(-direction.y, direction.x);
                float zigzagOffset = Mathf.Sin(timeElapsed * zigzagFrequency) * zigzagAmplitude * radius;
                if (secondBullet)
                {
                    zigzagOffset = Mathf.Sin(timeElapsed * zigzagFrequency + Mathf.PI) * zigzagAmplitude * radius;
                }

                Vector2 newPosition = startPosition + moveDirection + perpendicularDirection * zigzagOffset;
                transform.position = newPosition;
            }
        }

        void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}
