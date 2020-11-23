using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsUtility
{
    public static float VelocityFromSpaceAndTime(float s, float t)
    {
        return s / t;
    }
    
    public static float SpaceFromVelocityAndTime(float v, float t)
    {
        return v * t;
    }
    
    public static float TimeFromSpaceAndVelocity(float s, float v)
    {
        return s / v;
    }
}