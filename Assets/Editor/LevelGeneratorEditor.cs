using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate")) {
            ((LevelGenerator)target)?.GenerateTheFloor();
            SceneView.RepaintAll();
        }

        if (GUILayout.Button("Clear")) {
            ((LevelGenerator)target)?.Clear();
            SceneView.RepaintAll();
        }
        EditorGUILayout.EndHorizontal();
    }
}