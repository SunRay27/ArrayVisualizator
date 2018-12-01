using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpdateSystem))]
public class UpdateSystemEditor : Editor
{

    SerializedProperty array;

    void OnEnable()
    {
        array = serializedObject.FindProperty("itemsToUpdate");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.HelpBox($"Registered behaviours: {array.arraySize}", MessageType.Info);
    }
}
