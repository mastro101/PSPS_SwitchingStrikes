using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtility
{
    public static Vector2 FromV3ToV2XY(Vector3 _vector3)
    {
        return new Vector2(_vector3.x, _vector3.y);
    }
    
    public static Vector2 FromV3ToV2XZ(Vector3 _vector3)
    {
        return new Vector2(_vector3.x, _vector3.z);
    }
    
    public static Vector2 FromV3ToV2ZY(Vector3 _vector3)
    {
        return new Vector2(_vector3.z, _vector3.y);
    }

    public static Vector3 FromV2ToV3XYZ(Vector2 _vector2, float z = 0)
    {
        return new Vector3(_vector2.x, _vector2.y, z);
    }
    
    public static Vector3 FromV2ToV3XZY(Vector2 _vector2, float y = 0)
    {
        return new Vector3(_vector2.x, y, _vector2.y);
    }
    
    public static Vector3 FromV2ToV3ZYX(Vector2 _vector2, float x = 0)
    {
        return new Vector3(x, _vector2.y, _vector2.x);
    }

    public static Vector3 FromV2ToV3ZXY(Vector2 _vector2, float y = 0)
    {
        return new Vector3(_vector2.y, y, _vector2.x);
    }

    public static Vector3 NormalizeWithPrecision(Vector3 _vector3, int _decimal)
    {
        float x = _vector3.x, y = _vector3.y, z = _vector3.z;
        Vector3 v = _vector3 / Mathf.Sqrt(x * x + y * y + z * z);
        Vector3 normal = Approximation(v, _decimal);
        Debug.LogFormat("Normal of {0} with {1} decimal = {2}", _vector3, _decimal, normal);
        return normal;
    }

    public static Vector3 Approximation(Vector3 _vector3, int _decimal)
    {
        return new Vector3(MathUtility.Approximation(_vector3.x, _decimal), MathUtility.Approximation(_vector3.y, _decimal), MathUtility.Approximation(_vector3.z, _decimal));
    }
}