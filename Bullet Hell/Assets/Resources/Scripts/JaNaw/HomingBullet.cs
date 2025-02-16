using System.Collections;
using UnityEngine;

namespace Janaw
{
    public class HomingBullet : MonoBehaviour
    {
        public Transform player;

        public Vector2 startPoint;
        public Vector2 controlPoint;
        public Vector2 endPoint;

        public float travelTime = 1.5f; // Time to complete Bezier movement
        public float homingSpeed = 6f;
        public float homingDelay = 0.5f;

        private float elapsedTime = 0f;
        private bool isHoming = false;
        private bool canHome = true;
        private Vector2 targetPosition;
        private Vector2 homingDirection;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

            if (player == null)
            {
                Debug.LogWarning("Player not found! Homing bullets won't work.");
                Destroy(gameObject, 5f);
                return;
            }

            // Set up the Bezier curve points
            startPoint = transform.position;
            endPoint = startPoint + new Vector2(-5f, Random.Range(-2f, 2f)); // Moves left with slight variation
            controlPoint = (startPoint + endPoint) / 2 + new Vector2(0, Random.Range(-3f, 3f)); // Adds wave-like curve

            StartCoroutine(StartHoming());
        }

        void Update()
        {
            if (!isHoming)
            {
                // Move along the Bezier curve
                elapsedTime += Time.deltaTime / travelTime;
                transform.position = CalculateBezierPoint(elapsedTime, startPoint, controlPoint, endPoint);

                if (elapsedTime >= 1f)
                {
                    isHoming = true; // Transition to homing phase
                }
            }
            else if (canHome)
            {
                // Move in the direction of the target
                transform.position += (Vector3)(homingDirection * homingSpeed * Time.deltaTime);
            }
        }

        IEnumerator StartHoming()
        {
            yield return new WaitForSeconds(travelTime + homingDelay);

            if (player != null)
            {
                targetPosition = player.position;
                homingDirection = (targetPosition - (Vector2)transform.position).normalized;
                isHoming = true;
            }
            else
            {
                canHome = false;
            }
        }

        // Calculate position on a quadratic Bezier curve
        Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
        {
            return Mathf.Pow(1 - t, 2) * p0 +
                   2 * (1 - t) * t * p1 +
                   Mathf.Pow(t, 2) * p2;
        }

        void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}
