using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PolygonGenerator))]
public class PolygonGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PolygonGenerator script = (PolygonGenerator)target;

        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Generate"))
        {
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Vaffanculo");
                script.GenerateMesh();
            }
        }
    }
}