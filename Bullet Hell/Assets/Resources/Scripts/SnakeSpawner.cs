using System.Collections.Generic;
using UnityEngine;

public class SnakeSpawner : MonoBehaviour
{
    public GameObject snakeSegmentPrefab; // Snake segment prefab.
    public int initialSegmentCount = 5; // How many segments to start with.

    private List<SnakeSegments> segments = new List<SnakeSegments>(); // List of all snake segments.
    void Start()
    {
        SpawnSnake();
    }

    void SpawnSnake()
    {
        //prevSegment = transform; // Start from the spawner position.

        // Spawn the initial snake head
        //GameObject head = Instantiate(snakeSegmentPrefab, lastSegment.position, Quaternion.identity);
        //lastSegment = head.transform; // Set the last segment to the head.
        //head.AddComponent<SnakeHead>();

        // Spawn the rest of the snake segments.
        for (int i = 0; i < initialSegmentCount; i++)
        {
            GameObject segment; 
            if (i == 0)
            {
                segment = Instantiate(snakeSegmentPrefab, transform.position, Quaternion.identity);
                segments.Add(segment.GetComponent<SnakeSegments>());
                segment.AddComponent<SnakeHead>();
            }
            else
            {
                float offset = segments[i - 1].GetComponent<SpriteRenderer>().bounds.size.x;
                Vector3 pos = segments[i - 1].transform.position + new Vector3(offset, 0, 0);
                segment = Instantiate(snakeSegmentPrefab, pos, Quaternion.identity);
                segments.Add(segment.GetComponent<SnakeSegments>());
                segments[i].previousSegment = segments[i - 1].transform;
            }
        }
    }
}
