using UnityEngine;

public class SnakeSegments : MonoBehaviour
{
    [HideInInspector] public Transform previousSegment = null; // Reference to the previous segment.
    [HideInInspector] public float offset;


    private void Start()
    {
        offset = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
    }


    private void Update()
    {
        if (previousSegment != null) 
        {
            Vector3 targetDir = previousSegment.position - transform.position;
            targetDir.Normalize();

            Vector3 targetPos = previousSegment.position + targetDir * offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, 5 * Time.deltaTime);
        }
    }
}
