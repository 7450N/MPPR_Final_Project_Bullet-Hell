using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//Written by Jason

namespace MPPR
{
    public static class CustomMethod // A static class to store custom methods
    {
        public static void Translation(this Transform transform, Vector3 translation, Space relativeTo = Space.Self) // A translation method without using Unity Built-in method
        {
            if (relativeTo == Space.Self)
            {
                transform.position += transform.rotation * translation;
            }
            else // Space.World
            {
                transform.position += translation;
            }
        }


        public static float Clamp(this float value, float min, float max) // A clamp method without using Unity Built-in method
        {
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }

        public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
        {
            // Calculate the vector from current to target
            Vector3 direction = target - current;

            // Get the distance to the target
            float distance = direction.magnitude;

            // If the object is already at or very close to the target, return the target position
            if (distance <= maxDistanceDelta || distance == 0f)
            {
                return target;
            }

            // Normalize the direction vector to move at a constant speed
            return current + direction.normalized * maxDistanceDelta;
        }


        public static float CalculateDistance(Vector3 a, Vector3 b)      //calculate the distance between 2 vectors by using pythagorus theorem
        {
            float squareSum = (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
            return Mathf.Sqrt(squareSum);          //return distance
        }


        public static float DotProduct(Vector3 a, Vector3 b) // A dot product method for vectors without using Unity Built-in method
        {
            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }


        public static float DotProduct(Quaternion a, Quaternion b) // A dot product method for quaternion without using Unity Built-in method
        {
            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }


        public static Vector3 Slerp(Vector3 start, Vector3 end, float t)      //Spherical Interpolatiion method for vector using the formula Slerp(A,B,t) = (sin((1-t)θ) / sin(θ)) * A + (sin(tθ) / sin(θ)) * B
        {   
            Vector3 a = start.normalized;
            Vector3 b = end.normalized;

            float dot = DotProduct(a, b);
            dot = dot.Clamp(-1.0f, 1.0f);

            float theta = Mathf.Acos(dot);         
            //I am not going tto implement inverse cosine method as spherical interpolation and quaternion is killing me 
            float sinTheta = Mathf.Sin(theta);          

            if (sinTheta < 0.001f)  return Lerp(start, end, t); // Linear interpolation for small angles

            float weight1 = Mathf.Sin((1 - t) * theta) / sinTheta;      //separate the formula into smaller parts
            float weight2 = Mathf.Sin(t * theta) / sinTheta;

            return weight1 * start + weight2 * end;
        }


        public static Quaternion Slerp(Quaternion q1, Quaternion q2, float t)  //Spherical Interpolation method for quaternion using the formula Slerp(q1,q2,t) = (sin((1-t)θ) / sin(θ)) * q1 + (sin(tθ) / sin(θ)) * q2
        {
            float dot = DotProduct(q1, q2);       //calculate dot product of 2 quaternions
     
            if (dot < 0.0f)                 // If the dot product is negative, negate one quaternion to take the shortest path
            {
                q1 = new Quaternion(-q1.x, -q1.y, -q1.z, -q1.w);
                dot = -dot;
            }

            dot = dot.Clamp(-1.0f, 1.0f);       // Clamp dot product to avoid precision errors
            float theta = Mathf.Acos(dot);      // Compute the angle between quaternions

            
            if (theta < 0.001f) return Lerp(q1, q2, t);  //linear interpolation for small angles

            // Compute the spherical interpolation factors
            float sinTheta = Mathf.Sin(theta);
            float weight1 = Mathf.Sin((1 - t) * theta) / sinTheta;
            float weight2 = Mathf.Sin(t * theta) / sinTheta;

            // Compute the final quaternion (In quaternion, multiplying is equivalent to adding rotations) and also quaternion is 4D vector.
            return new Quaternion( 
                weight1 * q1.x + weight2 * q2.x,
                weight1 * q1.y + weight2 * q2.y,
                weight1 * q1.z + weight2 * q2.z,
                weight1 * q1.w + weight2 * q2.w
            );
        }

        public static Vector3 Lerp(Vector3 start, Vector3 end, float t) //Linear Interpolation method for vector using the formula Lerp(A,B,t) = (1-t)A + tB
        {
            return (1 - t) * start + t * end;       //return the linear interpolation of 2 vectors
        }

        public static Quaternion Lerp(Quaternion start, Quaternion end, float t) //Linear Interpolation method for quaternion using the formula Lerp(q1,q2,t) = (1-t)q1 + tq2
        {
            return new Quaternion(
                (1 - t) * start.x + t * end.x,
                (1 - t) * start.y + t * end.y,
                (1 - t) * start.z + t * end.z,
                (1 - t) * start.w + t * end.w
            );
        }

        public static Vector3 BezierCurve(Vector3 start, Vector3 end, int controlPointCount, float t)
        {
            // Generate a list of controls points and add the start as the first element of the list
            List<Vector3> controlPoints = new() { start };

            for (int i = 0; i < controlPointCount; i++)
            {
                Vector3 randomPoint = new(Random.Range(SnakeConstants.XMinRange, SnakeConstants.XMaxRange),
                                          Random.Range(SnakeConstants.YMinRange, SnakeConstants.YMaxRange),
                                          0); // Generate a random control point
                controlPoints.Add(randomPoint);       //add the control point to the list
            }

            controlPoints.Add(end);      //add the end point to the list

            // Calculate the position along the curve using De Casteljau’s Algorithm
            return DeCasteljau(controlPoints, t);
        }

        private static Vector3 DeCasteljau(List<Vector3> points, float t)
        {
            int count = points.Count;
            for (int level = 1; level < count; level++)
            {
                for (int i = 0; i < count - level; i++)
                {
                    points[i] = Lerp(points[i], points[i + 1], t);
                }
            }
            return points[0];
        }

    }
}
