using System.Collections;
using UnityEngine;
namespace Gabriel{
    public class Pattern1Movement : MonoBehaviour
    {
        public float amplitude = 5f;
        public float speed = 1f;
        private float time = 0f;
        private Vector3 startPosition;

        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
            time += speed * Time.deltaTime;
            float x = amplitude * Mathf.Cos(time);
            float y = (amplitude / 2) * Mathf.Sin(2 * time);
            transform.position = startPosition + new Vector3(x, y, 0);
        }
    }
}