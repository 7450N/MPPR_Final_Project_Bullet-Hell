using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Jason

namespace Jason
{
    public class Turrent : MonoBehaviour
    {
        [Range(20f, 100f)] private float rotationSpeed = 50f; // Rotation speed of the turrent
        private int dir;
        // Start is called before the first frame update
        void Start()
        {
            rotationSpeed = Random.Range(20f, 100f); // Randomize the rotation speed of the turrent
            dir = Random.Range(0, 2) * 2 - 1; // Randomize the direction of the rotation
        }

        // Update is called once per frame
        void Update()
        {
            //transform.rotation *= Quaternion.Euler(0, 0, rotationSpeed * dir * Time.deltaTime); // rotate in the z-axis
            transform.rotation *= Quaternion.Euler(0, rotationSpeed * dir * Time.deltaTime, 0); // rotate in the y-axis
        }
    }

}
