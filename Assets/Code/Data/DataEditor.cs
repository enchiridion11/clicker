using System;
using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR
public class DataEditor : EditorWindow {
    #region Fields

    public DataArrays DataArrays;

    const string DataPath = "/StreamingAssets/data.json";

    Vector2 scrollPosition = Vector2.zero;

    #endregion

    #region Methods

    #region Unity

    void OnGUI () {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

        if (DataArrays != null) {
            var serializedObject = new SerializedObject(this);
            var serializedProperty = serializedObject.FindProperty("DataArrays");
            EditorGUILayout.PropertyField(serializedProperty, true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save Data")) {
                SaveGameData();
            }
        }

        if (GUILayout.Button("Load Data")) {
            LoadGameData();
        }

        GUILayout.EndScrollView();
    }

    #endregion

    [MenuItem("Window/Data Editor")]
    static void Initialize () {
        GetWindow(typeof(DataEditor)).Show();
    }

    void LoadGameData () {
        var dataFilePath = Application.dataPath + DataPath;

        if (File.Exists(dataFilePath)) {
            var dataAsJson = File.ReadAllText(dataFilePath);
            DataArrays = JsonUtility.FromJson<DataArrays>(dataAsJson);
            Debug.Log("Loaded existing data!");
        }
        else {
            DataArrays = new DataArrays();
            Debug.Log("Created new data!");
        }
    }

    void SaveGameData () {
        var dataAsJson = JsonUtility.ToJson(DataArrays);

        var filePath = Application.dataPath + DataPath;
        try {
            File.WriteAllText(filePath, dataAsJson);
        }
        catch (Exception e) {
            Debug.LogError(e);
            return;
        }

        Debug.Log("Data saved successfully!");
    }

    #endregion
}
#endif