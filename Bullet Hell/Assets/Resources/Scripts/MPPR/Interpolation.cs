using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MPPR
{
    public static class Interpolator
    {
        public static Vector3 Lerp(Vector3 start, Vector3 end, float t)
        {
            return start + (end - start) * t;
        }
    }
}