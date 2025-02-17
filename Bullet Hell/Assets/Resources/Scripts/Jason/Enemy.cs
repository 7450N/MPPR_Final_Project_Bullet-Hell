using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jason
{
    public class Enemy : MonoBehaviour
    {   
        private Snake snake;

        void Start()
        {
            snake = GameObject.Find("Snake").GetComponent<Snake>();
        }

        public float GetRadius()
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                return meshRenderer.bounds.extents.x; // Extents give half of the width
            }
            return 0f;
        }

        public void TakeDamage()
        {
            if (snake == null) return; // If the snake is null, return
            snake.enemyHp -= 5; // Decrease enemy health by 1
        }
    }

}

