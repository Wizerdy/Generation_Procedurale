using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Gates))]
public class GatesPropertyDrawer : PropertyDrawer {
    Rect _toggle = new Rect(0f, 0f, 15f, 15f);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        SerializedProperty int4Prop = property.FindPropertyRelative("value");
        SerializedProperty boolArrayProp = int4Prop.FindPropertyRelative("value");

        EditorGUI.LabelField(position, property.displayName);

        _toggle.position = position.position;
        _toggle.x += position.width - _toggle.width * 3f;

        Rect[] toggles = { new Rect(_toggle.position, _toggle.size),
            new Rect(_toggle.position + new Vector2(_toggle.width, _toggle.height), _toggle.size),
            new Rect(_toggle.position + new Vector2(0, _toggle.height * 2f), _toggle.size),
            new Rect(_toggle.position + new Vector2(-_toggle.width, _toggle.height), _toggle.size) };

        for (int i = 0; i < boolArrayProp.arraySize; i++) {
            DrawToggle(boolArrayProp.GetArrayElementAtIndex(i), toggles[i]);
        }
    }

    private void DrawToggle(SerializedProperty property, Rect rect) {
        property.boolValue = EditorGUI.Toggle(rect, property.boolValue);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return /*EditorGUIUtility.singleLineHeight * 2f*/45f;
    }
}
