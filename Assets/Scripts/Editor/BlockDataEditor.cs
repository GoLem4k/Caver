using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BlockData))]
public class BlockDataEditor : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();

        var nameProp = serializedObject.FindProperty("blockName");
        var healthProp = serializedObject.FindProperty("health");

        if (nameProp == null || healthProp == null) {
            EditorGUILayout.HelpBox("Поля не найдены! Проверь имена полей.", MessageType.Error);
            return;
        }

        EditorGUILayout.PropertyField(nameProp);

        if (healthProp.propertyType == SerializedPropertyType.Integer) {
            EditorGUILayout.IntSlider(healthProp, 0, 100);
        } else if (healthProp.propertyType == SerializedPropertyType.Float) {
            EditorGUILayout.Slider(healthProp, 0f, 100f);
        } else {
            EditorGUILayout.PropertyField(healthProp);
        }

        if (GUILayout.Button("Сбросить")) {
            if (healthProp.propertyType == SerializedPropertyType.Integer) {
                healthProp.intValue = 100;
            } else if (healthProp.propertyType == SerializedPropertyType.Float) {
                healthProp.floatValue = 100f;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}