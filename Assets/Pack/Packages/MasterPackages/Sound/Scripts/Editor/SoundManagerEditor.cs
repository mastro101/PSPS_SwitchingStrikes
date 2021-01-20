using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    SoundManager c;

    private void OnEnable()
    {
        c = (SoundManager)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //c.UpdateVolumes();
    }
}