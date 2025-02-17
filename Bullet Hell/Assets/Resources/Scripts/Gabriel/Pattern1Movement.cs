using System.Collections;
using UnityEngine;

namespace Gabriel
{
    public class Pattern1Movement : MonoBehaviour
    {
        public float amplitude = 5f; // Amplitude of the movement along the X and Y axes
        public float speed = 1f; // Speed of the movement
        private float time = 0f; // Time variable to control the movement's progression
        private Vector3 startPosition; // Initial position of the object

        void Start()
        {
            // Store the starting position of the object when the game starts
            startPosition = transform.position;
        }

        void Update()
        {
            time += speed * Time.deltaTime;
            //lemniscate of Gerono
            float x = amplitude * Mathf.Cos(time); 
            float y = (amplitude / 2) * Mathf.Sin(2 * time); 

            transform.position = startPosition + new Vector3(x, y, 0);
        }
    }
}
