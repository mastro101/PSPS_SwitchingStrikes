using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ViewTriggerFromTransform))]
public class ViewTriggerFromTransformEditor : Editor
{
    SerializedProperty radius;
    SerializedProperty viewAngle;

    ViewTriggerFromTransform c;
    Vector3 handlePos;

    private void OnEnable()
    {
        c = (ViewTriggerFromTransform)target;

        radius = serializedObject.FindProperty("radius");
        viewAngle = serializedObject.FindProperty("viewAngle");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        radius.floatValue = EditorGUILayout.FloatField(radius.displayName, radius.floatValue);
        viewAngle.floatValue = EditorGUILayout.Slider(viewAngle.displayName, viewAngle.floatValue, 0f, 360f);

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        bool trigger = false;
        Vector3 arcSideDir = Quaternion.AngleAxis(-viewAngle.floatValue / 2f, c.transform.up) * c.transform.forward;
        Vector3 newHandlePos = c.transform.position + arcSideDir * radius.floatValue;

        handlePos = Handles.PositionHandle(newHandlePos, c.transform.rotation);
        Vector3 localHandlePos = handlePos - c.transform.position;


        radius.floatValue = localHandlePos.magnitude;
        viewAngle.floatValue = Vector3.Angle(c.transform.forward, localHandlePos) * 2f;

        serializedObject.ApplyModifiedProperties();

        if (c.CheckTrigger())
        {
            Handles.color = Color.green;
            trigger = true;
        }
        else
            Handles.color = Color.white;

        HandlesUtility.DrawArc3D(viewAngle.floatValue, radius.floatValue, c.transform.position, c.transform.up, c.transform.forward);

        if (c.objTransform)
        {
            if (c.CheckDistance(c.objTransform) && c.CheckAngle(c.objTransform))
            {
                if (trigger)
                    Handles.color = Color.green;
                else
                    Handles.color = Color.red;

                Handles.DrawLine(c.transform.position, c.objTransform.position);
            }
        }
    }
}