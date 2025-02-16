using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPPR;

namespace Jason
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float xMinRange = -17f; // Minimum X value for the player to move
        [SerializeField] private float xMaxRange = 17f; // Maximum X value for the player to move
        [SerializeField] private float zMinRange = -9f; // Minimum Z value for the player to move
        [SerializeField] private float zMaxRange = 9f; // Maximum Z value for the player to move
        [SerializeField] private float speed = 5f; // Speed of the player
        float xInput;
        float zInput;

        // Start is called before the first frame update
        void Start()
        {
            PlayerConstants.XMaxRange = xMaxRange;           
            PlayerConstants.XMinRange = xMinRange;
            PlayerConstants.ZMaxRange = zMaxRange;
            PlayerConstants.ZMinRange = zMinRange;
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

            // to make the player not to go out of bound
            Vector3 clampedPos = transform.position;
            clampedPos.x = clampedPos.x.Clamp(xMinRange, xMaxRange);
            clampedPos.z = clampedPos.z.Clamp(zMinRange, zMaxRange);
            transform.position = clampedPos;
        }
    }

}
