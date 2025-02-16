using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPPR;

//Written by Jason
namespace Jason
{
    public class Snake : MonoBehaviour
    {
        public List<Transform> BodyParts = new List<Transform>(); // A list of snake body parts

        public float minDistance = 1f; // Minimum distance between body parts
        public float speed = 5; // Speed of the snake

        [Range(0, 20)] public int numBodyParts = 1; // Number of body parts the snake has
        public GameObject bodyPrefab; // the prefab used to create the snake body parts

        private Transform currentPart; // the current body part
        private Transform previousPart; // the previous body part
        private float distance;

        [SerializeField] private float xMaxRange = 16.5f;
        [SerializeField] private float xMinRange = -16.5f;
        [SerializeField] private float yMaxRange = 8.5f;
        [SerializeField] private float yMinRange = -8.5f;

        private Vector3 targetPos;

        // Start is called before the first frame update
        void Start()
        {
            SnakeConstants.YMaxRange = yMaxRange;
            SnakeConstants.YMinRange = yMinRange;
            SnakeConstants.XMaxRange = xMaxRange;
            SnakeConstants.XMinRange = xMinRange;
            for (int i = 0; i < numBodyParts; i++)
            {
                AddBodyParts();
            }
            SetRandomPos();
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }


        public void Move()
        {
            //BodyParts[0].Translation(speed * Time.smoothDeltaTime * BodyParts[0].up, Space.World); // Move the head of the snake
            BodyParts[0].position = CustomMethod.MoveTowards(BodyParts[0].position, targetPos, speed * Time.smoothDeltaTime);

            if (CustomMethod.CalculateDistance(BodyParts[0].position, targetPos) < 0.1f)    // If the character is close to the target, set a new target and reset t
            {
                SetRandomPos();
            }

            for (int i = 1; i < BodyParts.Count; i++) // Move the body parts of the snake, starting from the second body part
            {
                currentPart = BodyParts[i];
                previousPart = BodyParts[i - 1];

                distance = CustomMethod.CalculateDistance(previousPart.position, currentPart.position);

                Vector3 newPosition = previousPart.position;
                newPosition.z = BodyParts[0].position.z;
                float T = (Time.deltaTime * distance / minDistance * speed).Clamp(0f, 0.5f);

                currentPart.SetPositionAndRotation(CustomMethod.Slerp(currentPart.position, newPosition, T), CustomMethod.Slerp(currentPart.rotation, previousPart.rotation, T));
            }
        }


        public void AddBodyParts()
        {
            Transform newPart = (Instantiate(bodyPrefab, BodyParts[^1].position, BodyParts[^1].rotation) as GameObject).transform; //^ accesses from the end of the list
            newPart.SetParent(transform); // add the new body part to the parent object
            BodyParts.Add(newPart); // add the new boddy part to the list
        }


        private void SetRandomPos()
        {
            targetPos.x = Random.Range(SnakeConstants.XMinRange, SnakeConstants.XMaxRange);
            targetPos.y = Random.Range(SnakeConstants.YMinRange, SnakeConstants.YMaxRange);
            targetPos.z = 0;
        }
    }
}

