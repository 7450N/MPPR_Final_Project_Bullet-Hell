using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPPR;

namespace Jason
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float speed = 5f; // Speed of the player
        float xInput;
        float zInput;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            GetInput();
            Move();
        }


        private void GetInput()
        {
            xInput = Input.GetAxis("Horizontal");
            zInput = Input.GetAxis("Vertical");
        }


        private void Move()
        {
            Vector3 moveDir = new Vector3(xInput, 0, zInput);

            if (moveDir != Vector3.zero) moveDir.Normalize();      //Normalize the vector to get consistent speed when moving diagonally
               
            transform.Translation(moveDir * speed * Time.deltaTime, Space.Self);
        }
    }

}
