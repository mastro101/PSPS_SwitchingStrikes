using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class HandlesUtility
{
    public static void DrawArc(float angle, float radius, Vector3 center, Vector3 normal, Vector3 forward)
    {
        normal.Normalize();
        forward.Normalize();
        Vector3 arcSideDir = Quaternion.AngleAxis(-angle / 2f, normal) * forward;
        Handles.DrawWireArc(center, normal, arcSideDir, angle, radius);
        Handles.DrawLine(center, center + (arcSideDir * radius));
        Handles.DrawLine(center, center + Vector3.Reflect(-(arcSideDir * radius), forward));
    }

    public static void DrawArc3D(float angle, float radius, Vector3 center, Vector3 normal, Vector3 forward)
    {
        normal.Normalize();
        forward.Normalize();
        DrawArc(angle, radius, center, normal, forward);
        DrawArc(angle, radius, center, Quaternion.AngleAxis(90f, forward) * normal, forward);
        Vector3 arcSide = Quaternion.AngleAxis(-angle / 2f, normal) * (forward * radius);
        Handles.DrawWireArc(center, forward, arcSide.normalized, 360f, radius);
    }
}