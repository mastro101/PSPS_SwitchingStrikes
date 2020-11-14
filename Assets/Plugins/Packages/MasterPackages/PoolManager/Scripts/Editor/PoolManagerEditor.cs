using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

[CustomEditor(typeof(PoolManager))]
public class PoolManagerEditor : Editor
{
    SerializedProperty serializePoolable;

    PoolManager pool;

    private void OnEnable()
    {
        serializePoolable = serializedObject.FindProperty("serializePoolable");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        pool = (PoolManager)target;
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializePoolable);

        if (serializePoolable.objectReferenceValue != null)
        {
            GameObject obj = serializePoolable.objectReferenceValue as GameObject;
            IPoolable tempPoolable = obj.GetComponent<IPoolable>();
            if (tempPoolable == null)
            {
                serializePoolable.objectReferenceValue = null;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}