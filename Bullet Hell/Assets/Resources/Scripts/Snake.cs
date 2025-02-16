using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPPR;

//Written by Jason

public class Snake : MonoBehaviour
{      
    public List<Transform> BodyParts = new List<Transform>(); // A list of snake body parts

    public float minDistance = 0.5f; // Minimum distance between body parts

    public float speed = 1; // Speed of the snake
    public float rotationSpeed = 50; // Rotation speed of the snake
    [Range(0, 10)] public int numBodyParts = 1; // Number of body parts the snake has

    public GameObject bodyPrefab; // the prefab used to create the snake body parts
    private Transform currentPart; // the current body part
    private Transform previousPart; // the previous body part
    private float distance;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numBodyParts; i++)
        {
            AddBodyParts();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    public void Move()
    {
        BodyParts[0].Translation(speed * Time.smoothDeltaTime * BodyParts[0].up, Space.World); // Move the head of the snake
        for (int i = 1; i < BodyParts.Count; i++) // Move the body parts of the snake, starting from the second body part
        {
            currentPart = BodyParts[i];
            previousPart = BodyParts[i - 1];

            distance = CustomMethod.CalculateDistance(previousPart.position, currentPart.position);

            Vector3 newPosition = previousPart.position;
            newPosition.z = BodyParts[0].position.z;
            float T = (Time.deltaTime * distance / minDistance * speed).Clamp(0f, 0.5f);

            currentPart.SetPositionAndRotation(CustomMethod.Slerp(currentPart.position, newPosition, T), CustomMethod.Slerp(currentPart.rotation, previousPart.rotation, T));
            //currentPart.SetPositionAndRotation(Vector3.Slerp(currentPart.position, newPosition, T), Quaternion.Slerp(currentPart.rotation, previousPart.rotation, T));
            //Debug.Log("Vector 3: " + Vector3.Slerp(currentPart.position, newPosition, T));
           // Debug.Log("Custom: " + CustomMethod.Slerp(currentPart.position, newPosition, T));
        }
    }


    public void AddBodyParts()
    {
        Transform newPart = (Instantiate(bodyPrefab, BodyParts[^1].position, BodyParts[^1].rotation) as GameObject).transform; //^ accesses from the end of the list
        newPart.SetParent(transform); // add the new body part to the parent object
        BodyParts.Add(newPart); // add the new boddy part to the list
    }
}
