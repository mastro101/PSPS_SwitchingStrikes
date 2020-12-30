using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyTypeArrey))]
public class EnemyTypeArreyEditor : Editor
{
    SerializedProperty spawnData;
    int l;
    float[] possibilities;
    float[] newPossibilities;

    private void OnEnable()
    {
        spawnData = serializedObject.FindProperty("data");
        l = spawnData.arraySize;
        possibilities = new float[l];
        newPossibilities = new float[l];
        SetTempPossibilities(possibilities);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        SetTempPossibilities(possibilities);
        EditorGUILayout.PropertyField(spawnData);

        if (EditorGUI.EndChangeCheck())
        {
            SetTempPossibilities(newPossibilities);

            for (int i = 0; i < l; i++)
            {
                float oldP = possibilities[i], newP = possibilities[i];
                if (possibilities[i] != newPossibilities[i])
                {
                    SetTempPossibilitiesInPercent(i);
                    break;
                }
            }

            SetPossibilities();
            if (possibilities.ArraySum() != 100)
            {
                SetTempPossibilitiesInPercent();
                SetPossibilities();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    void SetTempPossibilities(float[] f)
    {
        for (int i = 0; i < l; i++)
        {
            f[i] = spawnData.GetArrayElementAtIndex(i).FindPropertyRelative("spawnProbability").floatValue;
        }
    }

    void SetTempPossibilitiesInPercent(int index)
    {
        float k = newPossibilities[index];
        float sum = 0f;
        float div = 0f;

        for (int i = 0; i < l; i++)
        {
            if (index != i)
            {
                sum += possibilities[i];
            }
        }
        
        div = sum / (100 - k);

        for (int i = 0; i < l; i++)
        {
            if (index == i)
            {
                possibilities[i] = newPossibilities[i];
            }
            else
                possibilities[i] /= div;
        }
    }

    void SetTempPossibilitiesInPercent()
    {
        float sum = possibilities.ArraySum();
        possibilities.DivideArray(sum / 100);
    }

    void SetPossibilities()
    {
        for (int i = 0; i < l; i++)
        {
            if (possibilities[i] >= 0)
                spawnData.GetArrayElementAtIndex(i).FindPropertyRelative("spawnProbability").floatValue = possibilities[i];
            else
                spawnData.GetArrayElementAtIndex(i).FindPropertyRelative("spawnProbability").floatValue = 0.1f;
        }
    }
}