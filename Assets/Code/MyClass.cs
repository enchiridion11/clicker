using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class MyClass {
    public string[] myArray;
}

[Serializable]
public class CameraShakePreset {
    [Serializable]
    public struct Shake {
        public float Frequency;
        public float Amplitude;
    }

    public Shake[] RotationalX;
    public Shake[] RotationalY;
    public Shake[] RotationalZ;
    public float Duration = 0.5f;
}

[CustomEditor(typeof(CameraShakePresentList))]
public class CameraShakePresentListEditor : Editor {
    public override void OnInspectorGUI () {
        EditorGUILayout.LabelField("Custom editor:");
        var serializedObject = new SerializedObject(target);
        var property = serializedObject.FindProperty("list");
        serializedObject.Update();
        EditorGUILayout.PropertyField(property, true);
        serializedObject.ApplyModifiedProperties();

       /* var myArrayProperty = serializedObject.FindProperty("list");
        var myElement = myArrayProperty.GetArrayElementAtIndex(0);
        Debug.Log(myElement.GetArrayElementAtIndex(0));
        myElement.objectReferenceValue = EditorGUILayout.ObjectField(myElement.objectReferenceValue, typeof(CameraShakePresentList), true);*/
    }
}