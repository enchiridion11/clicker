using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using WebSocketSharp;

#if UNITY_EDITOR
public class DataEditor : EditorWindow {
    #region Fields

    const string JsonPath = "/StreamingAssets/data.json";
    const string AssetPath = "Assets/Code/Data/itemDB.asset";

    enum State {
        BLANK,
        EDIT,
        ADD
    }

    List<string> itemPopUp;
    List<int> itemPopUpIndex = new List<int> ();

    State state;

    string newItemName;
    string newItemRequirementName;
    string oldItemName;

    int selectedItem;

    bool hasLooped;

    Vector2 scrollPositionList = Vector2.zero;
    Vector2 scrollPositionMain = Vector2.zero;

    Color colorDefault = Color.white;
    Color colorGreen = new Color (200 / 255f, 255 / 255f, 200 / 255f);
    Color colorRed = new Color (255 / 255f, 200 / 255f, 200 / 255f);

    // database
    ItemDatabase itemDb;

    SerializedObject itemDbObject;
    SerializedProperty itemDbList;

    // item class
    SerializedProperty id;
    SerializedProperty clicks;
    SerializedProperty requirementsArray;
    SerializedProperty requirementName;
    SerializedProperty requirementAmount;

    #endregion

    #region Methods

    #region Unity

    void OnEnable () {
        if (itemDb == null)
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
        itemDb = (ItemDatabase) AssetDatabase.LoadAssetAtPath (AssetPath, typeof(ItemDatabase));

        if (itemDb == null) {
            CreateDatabase ();
        }

        itemPopUp = new List<string> (itemDb.Count);

        itemDbObject = new SerializedObject (itemDb);
        itemDbList = itemDbObject.FindProperty ("database");

        for (var i = 0; i < itemDb.Count; i++) {
            AddItemToPopUpList (itemDb.Item (i).Id);
        }
    }

    void CreateDatabase () {
        itemDb = CreateInstance<ItemDatabase> ();
        AssetDatabase.CreateAsset (itemDb, AssetPath);
        AssetDatabase.SaveAssets ();
        AssetDatabase.Refresh ();
    }

    void DisplayListArea () {
        EditorGUILayout.BeginVertical (GUILayout.Width (200));
        EditorGUILayout.Space ();

        scrollPositionList = EditorGUILayout.BeginScrollView (scrollPositionList, "box", GUILayout.ExpandHeight (true));
        
        for (var i = 0; i < itemDb.Count; i++) {
            EditorGUILayout.BeginHorizontal ();
            GUI.color = colorRed;
            if (GUILayout.Button ("-", GUILayout.Width (20))) {
                if (state == State.ADD) {
                    return;
                }

                // make sure item is not being used as requirement in another item before deleting
                var itemUsage = GetItemUsage (itemDb.Item (i).Id);

                if (itemUsage.Count > 0) {
                    var list = "\n";
                    foreach (var t in itemUsage) {
                        list += "\n" + t;
                    }

                    EditorUtility.DisplayDialog ("Warning", "Cannot delete " + itemDb.Item (i).Id + ", it is being used in:" + list, "Ok");
                    return;
                }

                itemDb.RemoveAt (i);
                RemoveItemFromPopUpList (i);

                hasLooped = false;
                RefreshDatabase ();
                EditorUtility.SetDirty (itemDb);
                state = State.BLANK;
                return;
            }


            // ITEM IN LIST
            GUI.color = colorDefault;
            if (GUILayout.Button (itemDb.Item (i).Id, "box", GUILayout.ExpandWidth (true))) {
                hasLooped = false;

                itemPopUpIndex.Clear ();

                selectedItem = i;
                state = State.EDIT;
            }

            EditorGUILayout.EndHorizontal ();
        }

        EditorGUILayout.EndScrollView ();
        
        EditorGUILayout.BeginVertical (GUILayout.ExpandWidth (true));
        EditorGUILayout.LabelField ("Items: " + itemDb.Count, GUILayout.Width (75));

        GUI.color = colorGreen;
        if (GUILayout.Button ("New Item")) {
            if (state == State.ADD) {
                return;
            }

            hasLooped = false;

            itemDb.Add (new Item ("", 0, null));

            RefreshDatabase ();
            state = State.ADD;
        }
        
        EditorGUILayout.BeginHorizontal (GUILayout.ExpandWidth (true));

        GUI.color = colorDefault;
        if (GUILayout.Button ("Save JSON")) {
            SaveItemDatabase ();
        }
        
        GUI.color = colorDefault;
        if (GUILayout.Button ("Load JSON")) {
            LoadGameData ();
        }

        EditorGUILayout.EndHorizontal ();
        EditorGUILayout.EndVertical ();
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

    void DisplayAddMainArea () {
        // only run this once
        if (!hasLooped) {
            //clear fields
            if (id != null) {
                GUI.SetNextControlName ("Name");
                id.ClearArray ();
                GUI.FocusControl ("Name");
                requirementsArray.ClearArray ();
                itemPopUpIndex.Clear ();
            }

            hasLooped = true;
        }

        scrollPositionMain = EditorGUILayout.BeginScrollView (scrollPositionMain);

        var itemsArray = itemDbList.GetArrayElementAtIndex (itemDbList.arraySize - 1);
        id = itemsArray.FindPropertyRelative ("id");
        clicks = itemsArray.FindPropertyRelative ("clicks");
        requirementsArray = itemsArray.FindPropertyRelative ("requirements");
        
        EditorGUILayout.LabelField ("Name", EditorStyles.boldLabel);
        id.stringValue = EditorGUILayout.TextField (id.stringValue);
        
        EditorGUILayout.LabelField ("Clicks", EditorStyles.boldLabel);
        clicks.intValue = EditorGUILayout.IntField (clicks.intValue);

        if (itemDb.Count > 1) {
            GUI.color = colorGreen;
            if (GUILayout.Button ("Add Requirement")) {
                requirementsArray.InsertArrayElementAtIndex (requirementsArray.arraySize);
                itemPopUpIndex.Add (itemPopUpIndex.Count);
            }
        }

        for (var i = 0; i < requirementsArray.arraySize; i++) {
            requirementName = requirementsArray.GetArrayElementAtIndex (i).FindPropertyRelative ("item");
            requirementAmount = requirementsArray.GetArrayElementAtIndex (i).FindPropertyRelative ("amount");

            EditorGUILayout.LabelField ("Requirement " + (i + 1), EditorStyles.boldLabel);
            GUI.color = colorDefault;
            itemPopUpIndex[i] = EditorGUILayout.Popup ("Item:", itemPopUpIndex[i], GetItemPopUpList ());
            requirementName.stringValue = IndexToString (itemPopUpIndex[i]);
            requirementAmount.intValue = EditorGUILayout.IntField ("Amount", requirementAmount.intValue);

            GUI.color = colorRed;
            if (GUILayout.Button ("Remove Requirement")) {
                requirementsArray.DeleteArrayElementAtIndex (i);
            }
        }

        EditorGUILayout.Space ();

        GUI.color = colorDefault;
        if (GUILayout.Button ("Create", GUILayout.Width (100))) {
            // data validation
            if (id.stringValue.IsNullOrEmpty ()) {
                EditorUtility.DisplayDialog ("Error", "Name cannot be empty!", "Ok");
                return;
            }

            for (var i = 0; i < requirementsArray.arraySize; i++) {
                // data validation
                if (requirementsArray.GetArrayElementAtIndex (i).FindPropertyRelative ("amount").intValue == 0) {
                    EditorUtility.DisplayDialog ("Error", "Amount cannot be zero!", "Ok");
                    return;
                }
            }

            AddItemToPopUpList (id.stringValue);
            itemDb.Item (itemDb.Count - 1).Id = id.stringValue;
            itemDb.Item (itemDb.Count - 1).Clicks = clicks.intValue;
            itemDb.Item (itemDb.Count - 1).Requirements = SerializedArrayToList (requirementsArray);

            //clear fields
            GUI.SetNextControlName ("Name");
            id.ClearArray ();
            GUI.FocusControl ("Name");
            requirementsArray.ClearArray ();
            itemPopUpIndex.Clear ();

            RefreshDatabase ();
            EditorUtility.SetDirty (itemDb);
            hasLooped = false;
            state = State.BLANK;
        }

        GUI.color = colorDefault;
        if (GUILayout.Button ("Cancel", GUILayout.Width (100))) {
            //clear fields
            GUI.SetNextControlName ("Name");
            id.ClearArray ();
            GUI.FocusControl ("Name");
            requirementsArray.ClearArray ();
            itemPopUpIndex.Clear ();

            itemDb.RemoveAt (itemDb.Count - 1);

            hasLooped = false;
            state = State.BLANK;
        }

        EditorGUILayout.EndScrollView ();
    }

    void DisplayEditMainArea () {
        // only run this once
        if (!hasLooped) {
            var itemsArray = itemDbList.GetArrayElementAtIndex (selectedItem);

            id = itemsArray.FindPropertyRelative ("id");
            id.stringValue = itemDb.Item (selectedItem).Id;
            
            clicks = itemsArray.FindPropertyRelative ("clicks");
            clicks.intValue = itemDb.Item (selectedItem).Clicks;

            requirementsArray = itemsArray.FindPropertyRelative ("requirements");
            if (requirementsArray.arraySize > 0) {
                for (var i = 0; i < requirementsArray.arraySize; i++) {
                    requirementName = requirementsArray.GetArrayElementAtIndex (i).FindPropertyRelative ("item");
                    requirementAmount = requirementsArray.GetArrayElementAtIndex (i).FindPropertyRelative ("amount");

                    EditorGUILayout.LabelField ("Requirement " + (i + 1), EditorStyles.boldLabel);
                    itemPopUpIndex.Add (i);
                    GUI.color = colorDefault;
                    itemPopUpIndex[i] = EditorGUILayout.Popup ("Item:", StringToIndex (requirementName.stringValue), GetItemPopUpList ());
                    requirementAmount.intValue = itemDb.Item (selectedItem).Requirements[i].amount;
                }
            }

            hasLooped = true;
        }
        
        scrollPositionMain = EditorGUILayout.BeginScrollView (scrollPositionMain);

        // display selected item current name
        EditorGUILayout.LabelField ("New Name", EditorStyles.boldLabel);
        id.stringValue = EditorGUILayout.TextField (id.stringValue);
        
        EditorGUILayout.LabelField ("Clicks", EditorStyles.boldLabel);
        clicks.intValue = EditorGUILayout.IntField (clicks.intValue);

        if (itemDb.Count > 0) {
            GUI.color = colorGreen;
            if (GUILayout.Button ("Add Requirement")) {
                itemDb.Item (selectedItem).Requirements.Add (new ItemRequirements ("", 0));
                requirementsArray.InsertArrayElementAtIndex (requirementsArray.arraySize);
                itemPopUpIndex.Add (itemPopUpIndex.Count);
            }
        }

        if (requirementsArray.arraySize > 0) {
            for (var i = 0; i < requirementsArray.arraySize; i++) {
                requirementName = requirementsArray.GetArrayElementAtIndex (i).FindPropertyRelative ("item");
                requirementAmount = requirementsArray.GetArrayElementAtIndex (i).FindPropertyRelative ("amount");

                EditorGUILayout.LabelField ("Requirement " + (i + 1), EditorStyles.boldLabel);
                GUI.color = colorDefault;
                itemPopUpIndex[i] = EditorGUILayout.Popup ("Item:", itemPopUpIndex[i], GetItemPopUpList ());
                requirementName.stringValue = IndexToString (itemPopUpIndex[i]);
                requirementAmount.intValue = EditorGUILayout.IntField ("Amount", requirementAmount.intValue);

                GUI.color = colorRed;
                if (GUILayout.Button ("Remove Requirement")) {
                    if (EditorUtility.DisplayDialog ("Warning", "Are you sure you want to remove this requirement? This cannot be undone!", "Ok")) {
                        itemDb.Item (selectedItem).Requirements.RemoveAt (i);
                        requirementsArray.DeleteArrayElementAtIndex (i);
                    }
                }
            }
        }

        EditorGUILayout.Space ();

        GUI.color = colorDefault;
        if (GUILayout.Button ("Update", GUILayout.Width (100))) {
            // data validation
            if (id.stringValue.IsNullOrEmpty ()) {
                EditorUtility.DisplayDialog ("Error", "Name cannot be empty!", "Ok");
                return;
            }

            for (var i = 0; i < itemDb.Item (selectedItem).Requirements.Count; i++) {
                // data validation
                if (requirementName.stringValue == itemDb.Item (selectedItem).Id) {
                    EditorUtility.DisplayDialog ("Error", "Item cannot require itself!", "Ok");
                    return;
                }

                if (requirementAmount.intValue == 0) {
                    EditorUtility.DisplayDialog ("Error", "Amount cannot be zero!", "Ok");
                    return;
                }
            }

            oldItemName = itemDb.Item (selectedItem).Id;
            itemDb.Item (selectedItem).Id = id.stringValue;
            itemDb.Item (selectedItem).Clicks = clicks.intValue;
            itemDb.Item (selectedItem).Requirements = SerializedArrayToList (requirementsArray);

            if (oldItemName != id.stringValue) {
                UpdateItemPopUpList (id.stringValue, StringToIndex (oldItemName));
            }

            RefreshDatabase ();

            //clear fields
            GUI.SetNextControlName ("Name");
            id.ClearArray ();
            GUI.FocusControl ("Name");
            itemPopUpIndex.Clear ();

            EditorUtility.SetDirty (itemDb);
            hasLooped = false;
            state = State.BLANK;
        }

        GUI.color = colorDefault;
        if (GUILayout.Button ("Cancel", GUILayout.Width (100))) {
            for (var i = 0; i < itemDb.Item (selectedItem).Requirements.Count; i++) {
                // data validation
                if (requirementName.stringValue == itemDb.Item (selectedItem).Id) {
                    EditorUtility.DisplayDialog ("Error", "Item cannot require itself!", "Ok");
                    return;
                }

                if (requirementAmount.intValue == 0) {
                    EditorUtility.DisplayDialog ("Error", "Amount cannot be zero!", "Ok");
                    return;
                }
            }

            //clear fields
            GUI.SetNextControlName ("Name");
            id.ClearArray ();
            GUI.FocusControl ("Name");
            requirementsArray.ClearArray ();
            itemPopUpIndex.Clear ();

            hasLooped = false;
            state = State.BLANK;
        }
        
        EditorGUILayout.EndScrollView ();
    }

    void RefreshDatabase () {
        itemDbObject = new SerializedObject (itemDb);
        itemDbList = itemDbObject.FindProperty ("database");
        //itemDb.SortAlphabeticallyAtoZ ();
    }

    List<string> GetItemUsage (string item) {
        var itemsInUse = new List<string> ();
        for (var i = 0; i < itemDb.Count; i++) {
            foreach (var t in itemDb.Item (i).Requirements) {
                if (item == t.item) {
                    itemsInUse.Add (itemDb.Item (i).Id);
                }
            }
        }

        return itemsInUse;
    }

    void AddItemToPopUpList (string item) {
        itemPopUp.Add (item);
    }

    void RemoveItemFromPopUpList (int index) {
        itemPopUp.RemoveAt (index);
    }

    void UpdateItemPopUpList (string updatedItemName, int index) {
        var currentItemName = itemPopUp[index];
        for (var i = 0; i < itemDb.Count; i++) {
            if (itemDb.Item (i).Requirements.Count > 0) {
                for (var j = 0; j < itemDb.Item (i).Requirements.Count; j++) {
                    if (itemDb.Item (i).Requirements[j].item == currentItemName) {
                        itemDb.Item (i).Requirements[j].item = updatedItemName;
                    }
                }
            }
        }

        itemPopUp[index] = updatedItemName;
    }

    string[] GetItemPopUpList () {
        return itemPopUp.ToArray ();
    }

    string IndexToString (int index) {
        foreach (var t in itemPopUp) {
            if (itemPopUp[index] == t) {
                return itemPopUp[index];
            }
        }

        return null;
    }

    int StringToIndex (string item) {
        for (var i = 0; i < itemPopUp.Count; i++) {
            if (itemPopUp[i] == item) {
                return i;
            }
        }

        return 0;
    }

    List<ItemRequirements> SerializedArrayToList (SerializedProperty array) {
        var list = new List<ItemRequirements> ();
        foreach (SerializedProperty item in array) {
            list.Add (new ItemRequirements (item.FindPropertyRelative ("item").stringValue, item.FindPropertyRelative ("amount").intValue));
        }

        return list;
    }

    void LoadGameData () {
        var dataFilePath = Application.dataPath + JsonPath;

        if (File.Exists (dataFilePath)) {
            var dataAsJson = File.ReadAllText (dataFilePath);
            itemDb.database = DataConvert.JSONToItemDatabase (new JSONObject(dataAsJson)["database"]);
            EditorUtility.DisplayDialog ("Great Success", "Loaded existing data!", "Ok");
        }
        else {
            EditorUtility.DisplayDialog ("Error", "No json found!", "Ok");
        }
    }

    void SaveItemDatabase () {
        var dataAsJson = JsonUtility.ToJson (itemDb);

        var filePath = Application.dataPath + JsonPath;
        try {
            File.WriteAllText (filePath, dataAsJson);
        }
        catch (Exception e) {
            EditorUtility.DisplayDialog ("Error", "Data could not be saved! \n \n Error message:" + e, "Ok");
            return;
        }

        EditorUtility.DisplayDialog ("Great Success", "Data saved successfully!", "Ok");
    }
    
    //TODO: add saving of currency defaults

    #endregion
}
#endif