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

    public static Vector3 Approximation(Vector3 _vector3, int _decimal)
    {
        return new Vector3(MathUtility.Approximation(_vector3.x, _decimal), MathUtility.Approximation(_vector3.y, _decimal), MathUtility.Approximation(_vector3.z, _decimal));
    }
    
    public static Vector2 Approximation(Vector2 _vector2, int _decimal)
    {
        return new Vector2(MathUtility.Approximation(_vector2.x, _decimal), MathUtility.Approximation(_vector2.y, _decimal));
    }

    public static Vector4 NewVector4(Vector3 xyz, float w = 1)
    {
        return new Vector4(xyz.x, xyz.y, xyz.z, w);
    }
    
    public static Vector4 NewVector4(float x, Vector3 yzw)
    {
        return new Vector4(x, yzw.x, yzw.y, yzw.z);
    }

    public static Vector4 NewVector4(Vector2 xy, Vector2 zw)
    {
        return new Vector4(xy.x, xy.y, zw.x, zw.y);
    }

    public static Vector4 NewVector4(Vector2 xy, float z, float w)
    {
        return new Vector4(xy.x, xy.y, z, w);
    }

    public static Vector4 NewVector4(float x, Vector2 yz, float w)
    {
        return new Vector4(x, yz.x, yz.y, w);
    }

    public static Vector4 NewVector4(float x, float y, Vector2 zw)
    {
        return new Vector4(x, y, zw.x, zw.y);
    }
}