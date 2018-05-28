using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR
public class DataEditor : EditorWindow {
    #region Fields

    public DataArrays DataArrays;

    const string JsonPath = "/StreamingAssets/data.json";
    const string AssetPath = "Assets/Code/Data/itemDB.asset";

    enum State {
        BLANK,
        EDIT,
        ADD
    }

    List<string> itemPopUp;

    State state;

    string newItemName;

    int selectedItem;
    int itemPopUpIndex;

    ItemRequirementsDataObject[] newItemRequirements;

    ItemDatabase items;

    Vector2 scrollPosition = Vector2.zero;

    #endregion

    #region Methods

    #region Unity

    void OnEnable () {
        if (items == null)
            LoadDatabase ();

        state = State.BLANK;
    }

    void OnGUI () {
        EditorGUILayout.BeginHorizontal (GUILayout.ExpandWidth (true));
        DisplayListArea ();
        DisplayMainArea ();
        EditorGUILayout.EndHorizontal ();

        /*  if (DataArrays != null) {
              var serializedObject = new SerializedObject (this);
              var serializedProperty = serializedObject.FindProperty ("DataArrays");
              EditorGUILayout.PropertyField (serializedProperty, true);
  
              serializedObject.ApplyModifiedProperties ();
  
              if (GUILayout.Button ("Save Data")) {
                  SaveGameData ();
              }
          }
  
          if (GUILayout.Button ("Load Data")) {
              LoadGameData ();
          }*/

        // GUILayout.EndScrollView ();
    }

    #endregion

    [MenuItem ("Window/Data Editor")]
    static void Initialize () {
        GetWindow (typeof(DataEditor)).Show ();
    }

    void LoadDatabase () {
        items = (ItemDatabase) AssetDatabase.LoadAssetAtPath (AssetPath, typeof(ItemDatabase));
        itemPopUp = new List<string> (items.Count);


        if (items == null) {
            CreateDatabase ();
        }
    }

    string[] GetItemPopUpList () {
        itemPopUp.Add ("None");
        for (var i = 0; i < items.Count; i++) {
            itemPopUp.Add (items.Item (i).Name);
        }

        return itemPopUp.ToArray ();
    }

    void CreateDatabase () {
        items = CreateInstance<ItemDatabase> ();
        AssetDatabase.CreateAsset (items, AssetPath);
        AssetDatabase.SaveAssets ();
        AssetDatabase.Refresh ();
    }

    void DisplayListArea () {
        EditorGUILayout.BeginVertical (GUILayout.Width (250));
        EditorGUILayout.Space ();

        scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition, "box", GUILayout.ExpandHeight (true));

        for (var i = 0; i < items.Count; i++) {
            EditorGUILayout.BeginHorizontal ();
            if (GUILayout.Button ("-", GUILayout.Width (25))) {
                items.RemoveAt (i);
                items.SortAlphabeticallyAtoZ ();
                EditorUtility.SetDirty (items);
                state = State.BLANK;
                return;
            }

            if (GUILayout.Button (items.Item (i).Name, "box", GUILayout.ExpandWidth (true))) {
                selectedItem = i;
                state = State.EDIT;
            }

            EditorGUILayout.EndHorizontal ();
        }

        EditorGUILayout.EndScrollView ();

        EditorGUILayout.BeginHorizontal (GUILayout.ExpandWidth (true));
        EditorGUILayout.LabelField ("Items: " + items.Count, GUILayout.Width (100));

        if (GUILayout.Button ("New Item")) {
            state = State.ADD;
        }

        if (GUILayout.Button ("Save JSON")) {
            SaveGameData ();
        }

        EditorGUILayout.EndHorizontal ();
        EditorGUILayout.Space ();
        EditorGUILayout.EndVertical ();
    }

    void DisplayMainArea () {
        EditorGUILayout.BeginVertical (GUILayout.ExpandWidth (true));
        EditorGUILayout.Space ();

        switch (state) {
            case State.ADD:
                DisplayAddMainArea ();
                break;
            case State.EDIT:
                DisplayEditMainArea ();
                break;
            default:
                DisplayBlankMainArea ();
                break;
        }

        EditorGUILayout.Space ();
        EditorGUILayout.EndVertical ();
    }

    void DisplayBlankMainArea () {
        EditorGUILayout.LabelField (
            "- Click New Item to create a new item.\n" +
            "- Click an existing item to edit it.\n" +
            "- Click the minus button next to an item to remove it.\n" +
            "- Click Save JSON to save the JSON file.",
            GUILayout.ExpandHeight (true));
    }

    void DisplayEditMainArea () {
        items.Item (selectedItem).Name = EditorGUILayout.TextField (new GUIContent ("Name:"), items.Item (selectedItem).Name);
        //items.Item(selectedItem).damage = int.Parse(EditorGUILayout.TextField(new GUIContent("Damage: "), weapons.Weapon(selectedWeapon).damage.ToString()));

        EditorGUILayout.Space ();

        if (GUILayout.Button ("Update", GUILayout.Width (100))) {
            items.SortAlphabeticallyAtoZ ();
            EditorUtility.SetDirty (items);
            state = State.BLANK;
        }

        if (GUILayout.Button ("Cancel", GUILayout.Width (100))) {
            newItemName = string.Empty;
            //newWeaponDamage = 0;

            state = State.BLANK;
        }
    }

    void DisplayAddMainArea () {
        newItemName = EditorGUILayout.TextField (new GUIContent ("Name:"), newItemName);
        itemPopUpIndex = EditorGUILayout.Popup ("Requirements:", itemPopUpIndex, GetItemPopUpList ());
        
        var serializedObject = new SerializedObject (this);
        var serializedProperty = serializedObject.FindProperty ("ItemRequirements");
        EditorGUILayout.PropertyField (serializedProperty, true);
        serializedObject.ApplyModifiedProperties ();
        
        //newWeaponDamage = Convert.ToInt32(EditorGUILayout.TextField(new GUIContent("Damage: "), newWeaponDamage.ToString()));

        EditorGUILayout.Space ();

        if (GUILayout.Button ("Add", GUILayout.Width (100))) {
            items.Add (new Item (newItemName));
            items.SortAlphabeticallyAtoZ ();

            newItemName = string.Empty;
            //newWeaponDamage = 0;
            EditorUtility.SetDirty (items);
            state = State.BLANK;
        }

        if (GUILayout.Button ("Cancel", GUILayout.Width (100))) {
            newItemName = string.Empty;
            //newWeaponDamage = 0;

            state = State.BLANK;
        }
    }

    void LoadGameData () {
        var dataFilePath = Application.dataPath + JsonPath;

        if (File.Exists (dataFilePath)) {
            var dataAsJson = File.ReadAllText (dataFilePath);
            DataArrays = JsonUtility.FromJson<DataArrays> (dataAsJson);
            EditorUtility.DisplayDialog ("Great Success", "Loaded existing data!", "Ok");
        }
        else {
            DataArrays = new DataArrays ();
            EditorUtility.DisplayDialog ("Great Success", "Created new data!", "Ok");
        }
    }

    void SaveGameData () {
        var dataAsJson = JsonUtility.ToJson (items);

        var filePath = Application.dataPath + JsonPath;
        try {
            File.WriteAllText (filePath, dataAsJson);
        }
        catch (Exception e) {
            Debug.LogError (e);
            return;
        }

        EditorUtility.DisplayDialog ("Great Success", "Data saved successfully!", "Ok");
    }

    #endregion
}
#endif