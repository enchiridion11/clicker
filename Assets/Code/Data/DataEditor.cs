using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using WebSocketSharp;

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
    List<ItemRequirements> newItemRequirements = new List<ItemRequirements> ();
    List<int> itemPopUpIndex = new List<int> ();
    List<int> newItemRequirementAmount = new List<int> ();

    State state;

    string newItemName;
    string newItemRequirementName;
    string copiedName;

    int selectedItem;

    bool copiedValues;

    SerializedObject itemsDatabaseObject;
    SerializedProperty itemsList;

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
    }

    #endregion

    [MenuItem ("Window/Data Editor")]
    static void Initialize () {
        GetWindow (typeof(DataEditor)).Show ();
    }

    void LoadDatabase () {
        items = (ItemDatabase) AssetDatabase.LoadAssetAtPath (AssetPath, typeof(ItemDatabase));
        itemPopUp = new List<string> (items.Count);
        itemsDatabaseObject = new SerializedObject (items);
        itemsList = itemsDatabaseObject.FindProperty ("database");

        if (items == null) {
            CreateDatabase ();
        }
    }

    string[] GetItemPopUpList () {
        for (var i = 0; i < items.Count; i++) {
            itemPopUp.Add (items.Item (i).name);
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
                // make sure item is not being used as requirement in another item
                if (!CanRemoveItem (items.Item (i).name)) {
                    EditorUtility.DisplayDialog ("Warning", "Cannot delete " + items.Item (i).name + ", it is another items requirement!", "Ok");
                    return;
                }

                items.RemoveAt (i);

                // clear the name field
                GUI.SetNextControlName ("Name");
                newItemName = string.Empty;
                GUI.FocusControl ("Name");

                RefreshDatabase ();
                EditorUtility.SetDirty (items);
                state = State.BLANK;
                return;
            }

            if (GUILayout.Button (items.Item (i).name, "box", GUILayout.ExpandWidth (true))) {
                // reset names back to copied name
                if (copiedValues) {
                    GUI.SetNextControlName ("Name");
                    newItemName = copiedName;
                    GUI.FocusControl ("Name");
                    items.Item (selectedItem).name = copiedName;
                    copiedValues = false;
                }

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
        // store data for cancel
        if (!copiedValues) {
            copiedName = items.Item (selectedItem).name;
            copiedValues = true;
        }

        // display selected item current name
        items.Item (selectedItem).name = EditorGUILayout.TextField (new GUIContent ("Name:"), items.Item (selectedItem).name);

        // display selected item current requirements (if any)
        for (var i = 0; i < items.Item (selectedItem).requirements.Length; i++) {
            EditorGUILayout.LabelField ("Requirement " + (i + 1));
            // itemPopUpIndexUpdate = EditorGUILayout.Popup ("Item:", selectedItem, GetItemPopUpList ());
            items.Item (selectedItem).requirements[i].amount = EditorGUILayout.IntField (new GUIContent ("Amount:"), items.Item (selectedItem).requirements[i].amount);
        }


        EditorGUILayout.Space ();

        if (GUILayout.Button ("Update", GUILayout.Width (100))) {
            // data validation
            if (items.Item (selectedItem).name.IsNullOrEmpty ()) {
                EditorUtility.DisplayDialog ("Error", "Name cannot be empty!", "Ok");
                return;
            }

            // clear the name field
            GUI.SetNextControlName ("Name");
            newItemName = string.Empty;
            GUI.FocusControl ("Name");

            RefreshDatabase ();
            EditorUtility.SetDirty (items);
            copiedValues = false;
            state = State.BLANK;
        }

        if (GUILayout.Button ("Cancel", GUILayout.Width (100))) {
            // set back to original values
            items.Item (selectedItem).name = copiedName;

            // clear the name field
            GUI.SetNextControlName ("Name");
            newItemName = string.Empty;
            GUI.FocusControl ("Name");

            newItemRequirements.Clear ();
            itemPopUpIndex.Clear ();
            newItemRequirementAmount.Clear ();
            copiedValues = false;

            state = State.BLANK;
        }
    }

    void DisplayAddMainArea () {
        // display item name
        newItemName = EditorGUILayout.TextField (new GUIContent ("Name:"), newItemName);

        // display item requirements
        if (itemsList.arraySize > 0) {
            if (GUILayout.Button ("Add Requirement", GUILayout.MaxWidth (130), GUILayout.MaxHeight (20))) {
                newItemRequirements.Add (new ItemRequirements ("", 0));
                itemPopUpIndex.Add (0);
                newItemRequirementAmount.Add (0);
            }

            if (newItemRequirements.Count > 0) {
                for (var i = 0; i < newItemRequirements.Count; i++) {
                    EditorGUILayout.LabelField ("Requirement " + (i + 1));
                    itemPopUpIndex[i] = EditorGUILayout.Popup ("Item:", itemPopUpIndex[i], GetItemPopUpList ());
                    newItemRequirementAmount[i] = EditorGUILayout.IntField (new GUIContent ("Amount:"), newItemRequirementAmount[i]);

                    if (GUILayout.Button ("Remove Requirement")) {
                        newItemRequirements.RemoveAt (i);
                    }
                }
            }
        }

        EditorGUILayout.Space ();

        if (GUILayout.Button ("Create", GUILayout.Width (100))) {
            // data validation
            if (newItemName.IsNullOrEmpty ()) {
                EditorUtility.DisplayDialog ("Error", "Name cannot be empty!", "Ok");
                return;
            }

            for (var i = 0; i < newItemRequirements.Count; i++) {
                // data validation
                if (newItemRequirementAmount[i] == 0) {
                    EditorUtility.DisplayDialog ("Error", "Amount cannot be zero!", "Ok");
                    return;
                }

                newItemRequirements[i].item = itemPopUp[itemPopUpIndex[i]];
                newItemRequirements[i].amount = newItemRequirementAmount[i];
            }

            items.Add (new Item (newItemName, newItemRequirements.ToArray ()));

            RefreshDatabase ();

            newItemName = string.Empty;
            newItemRequirements.Clear ();
            itemPopUpIndex.Clear ();
            newItemRequirementAmount.Clear ();

            EditorUtility.SetDirty (items);
            state = State.BLANK;
        }

        if (GUILayout.Button ("Cancel", GUILayout.Width (100))) {
            // clear the name field
            GUI.SetNextControlName ("Name");
            newItemName = string.Empty;
            GUI.FocusControl ("Name");

            newItemRequirements.Clear ();
            itemPopUpIndex.Clear ();
            newItemRequirementAmount.Clear ();

            state = State.BLANK;
        }
    }

    void RefreshDatabase () {
        itemsDatabaseObject = new SerializedObject (items);
        itemsList = itemsDatabaseObject.FindProperty ("database");
        items.SortAlphabeticallyAtoZ ();
    }

    bool CanRemoveItem (string itemName) {
        for (var i = 0; i < items.Count; i++) {
            foreach (var t in items.Item (i).requirements) {
                if (itemName == t.item) {
                    return false;
                }
            }
        }

        return true;
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