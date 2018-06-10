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

    bool hasLooped;

    Vector2 scrollPosition = Vector2.zero;

    // database
    ItemDatabase itemDb;

    SerializedObject itemDbObject;
    SerializedProperty itemDbList;

    // item class
    SerializedProperty itemName;
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
    }

    void CreateDatabase () {
        itemDb = CreateInstance<ItemDatabase> ();
        AssetDatabase.CreateAsset (itemDb, AssetPath);
        AssetDatabase.SaveAssets ();
        AssetDatabase.Refresh ();
    }

    void DisplayListArea () {
        EditorGUILayout.BeginVertical (GUILayout.Width (250));
        EditorGUILayout.Space ();

        scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition, "box", GUILayout.ExpandHeight (true));

        for (var i = 0; i < itemDb.Count; i++) {
            EditorGUILayout.BeginHorizontal ();
            if (GUILayout.Button ("-", GUILayout.Width (25))) {
                // make sure item is not being used as requirement in another item
                var itemUsage = GetItemUsage (itemDb.Item (i).name);

                if (itemUsage.Count > 0) {
                    var list = "\n";
                    foreach (var t in itemUsage) {
                        list += "\n" + t;
                    }

                    EditorUtility.DisplayDialog ("Warning", "Cannot delete " + itemDb.Item (i).name + ", it is being used in:" + list, "Ok");
                    return;
                }

                itemDb.RemoveAt (i);
                RemoveItemFromPopUpList (i);

                // clear the name field
                GUI.SetNextControlName ("Name");
                newItemName = string.Empty;
                GUI.FocusControl ("Name");

                hasLooped = false;
                RefreshDatabase ();
                EditorUtility.SetDirty (itemDb);
                state = State.BLANK;
                return;
            }

            if (GUILayout.Button (itemDb.Item (i).name, "box", GUILayout.ExpandWidth (true))) {
                // reset names back to copied name
                if (hasLooped) {
                    GUI.SetNextControlName ("Name");
                    newItemName = copiedName;
                    GUI.FocusControl ("Name");
                    itemDb.Item (selectedItem).name = copiedName;

                    /*  for (var j = 0; j < itemsDb.Item (selectedItem).requirements.Length; j++) {
                          itemsDb.Item (selectedItem).requirements[j].item = newItemRequirements[j].item;
                          itemsDb.Item (selectedItem).requirements[j].amount = newItemRequirements[j].amount;
                      }*/

                    hasLooped = false;
                }

                newItemRequirements.Clear ();
                selectedItem = i;
                state = State.EDIT;
            }

            EditorGUILayout.EndHorizontal ();
        }

        EditorGUILayout.EndScrollView ();

        EditorGUILayout.BeginHorizontal (GUILayout.ExpandWidth (true));
        EditorGUILayout.LabelField ("Items: " + itemDb.Count, GUILayout.Width (100));

        if (GUILayout.Button ("New Item")) {
            hasLooped = false;

             itemDbList.InsertArrayElementAtIndex (itemDbList.arraySize);
            //itemDb.database.Add (new Item ("", new List<ItemRequirements> ()));
            RefreshDatabase ();

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
        // only run this code once
        if (!hasLooped) {
            copiedName = itemDb.Item (selectedItem).name;
            /* for (var i = 0; i < itemsDb.Item (selectedItem).requirements.Length; i++) {
                 itemPopUpIndex.Add (i);
                 var newItemRequirementsName = string.Empty;
                 for (var j = 0; j < itemPopUp.Count; j++) {
                     if (itemsDb.Item (selectedItem).requirements[i].item == itemPopUp[j]) {
                         itemPopUpIndex[i] = j;
                         newItemRequirementsName = itemPopUp[j];
                         break;
                     }
                 }
 
                 newItemRequirements.Add (new ItemRequirements (newItemRequirementsName, itemsDb.Item (selectedItem).requirements[i].amount));
             }*/

            hasLooped = true;
        }

        // display selected item current name
        itemDb.Item (selectedItem).name = EditorGUILayout.TextField (new GUIContent ("Name:"), itemDb.Item (selectedItem).name);

        // display selected item current requirements (if any)
        /*for (var i = 0; i < itemsDb.Item (selectedItem).requirements.Length; i++) {
            EditorGUILayout.LabelField ("Requirement " + (i + 1));
            itemPopUpIndex[i] = EditorGUILayout.Popup ("Item:", itemPopUpIndex[i], GetItemPopUpList ());
            itemsDb.Item (selectedItem).requirements[i].amount = EditorGUILayout.IntField (new GUIContent ("Amount:"), itemsDb.Item (selectedItem).requirements[i].amount);
        }*/


        EditorGUILayout.Space ();

        if (GUILayout.Button ("Update", GUILayout.Width (100))) {
            // data validation
            if (itemDb.Item (selectedItem).name.IsNullOrEmpty ()) {
                EditorUtility.DisplayDialog ("Error", "Name cannot be empty!", "Ok");
                return;
            }

            /* for (var i = 0; i < itemsDb.Item (selectedItem).requirements.Length; i++) {
                 itemsDb.Item (selectedItem).requirements[i].item = IndexToString (itemPopUpIndex[i]);
 
                 // data validation
                 if (itemsDb.Item (selectedItem).requirements[i].item == itemsDb.Item (selectedItem).name) {
                     EditorUtility.DisplayDialog ("Error", "Item cannot require itself!", "Ok");
                     return;
                 }
 
                 if (itemsDb.Item (selectedItem).requirements[i].amount == 0) {
                     EditorUtility.DisplayDialog ("Error", "Amount cannot be zero!", "Ok");
                     return;
                 }
             }*/

            // clear the name field
            GUI.SetNextControlName ("Name");
            newItemName = string.Empty;
            GUI.FocusControl ("Name");

            RefreshDatabase ();

            EditorUtility.SetDirty (itemDb);
            hasLooped = false;
            state = State.BLANK;
        }

        if (GUILayout.Button ("Cancel", GUILayout.Width (100))) {
            // set back to original values
            itemDb.Item (selectedItem).name = copiedName;

            /* for (var i = 0; i < itemsDb.Item (selectedItem).requirements.Length; i++) {
                 itemsDb.Item (selectedItem).requirements[i].item = newItemRequirements[i].item;
                 itemsDb.Item (selectedItem).requirements[i].amount = newItemRequirements[i].amount;
             }*/

            // clear the name field
            GUI.SetNextControlName ("Name");
            newItemName = string.Empty;
            GUI.FocusControl ("Name");

            newItemRequirements.Clear ();
            itemPopUpIndex.Clear ();
            newItemRequirementAmount.Clear ();
            hasLooped = false;

            state = State.BLANK;
        }
    }

    void DisplayAddMainArea () {
        var itemsArray = itemDbList.GetArrayElementAtIndex (itemDbList.arraySize - 1);
        itemName = itemsArray.FindPropertyRelative ("name");
        requirementsArray = itemsArray.FindPropertyRelative ("requirements");
        itemName.stringValue = EditorGUILayout.TextField ("Name", itemName.stringValue);

        if (!hasLooped) {
            //itemName.stringValue = string.Empty;
            hasLooped = true;
        }

        if (GUILayout.Button ("Add Requirement")) {
            requirementsArray.InsertArrayElementAtIndex (requirementsArray.arraySize);
        }

        for (var i = 0; i < requirementsArray.arraySize; i++) {
            requirementName = requirementsArray.GetArrayElementAtIndex (i).FindPropertyRelative ("item");
            requirementAmount = requirementsArray.GetArrayElementAtIndex (i).FindPropertyRelative ("amount");

            EditorGUILayout.LabelField ("Requirement " + (i + 1));
            requirementName.stringValue = EditorGUILayout.TextField ("Item", requirementName.stringValue);
            requirementAmount.intValue = EditorGUILayout.IntField ("Amount", requirementAmount.intValue);


            if (GUILayout.Button ("Remove Requirement")) {
                requirementsArray.DeleteArrayElementAtIndex (i);
            }
        }

        //Apply the changes to the list
        itemDbObject.ApplyModifiedProperties ();

        EditorGUILayout.Space ();

        if (GUILayout.Button ("Create", GUILayout.Width (100))) {
            // data validation
            if (itemName.stringValue.IsNullOrEmpty ()) {
                EditorUtility.DisplayDialog ("Error", "Name cannot be empty!", "Ok");
                return;
            }

            for (var i = 0; i < requirementsArray.arraySize; i++) {
                // data validation
                if (requirementsArray.GetArrayElementAtIndex (i).FindPropertyRelative ("item").stringValue.IsNullOrEmpty ()) {
                    EditorUtility.DisplayDialog ("Error", "Item cannot be empty!", "Ok");
                    return;
                }

                if (requirementsArray.GetArrayElementAtIndex (i).FindPropertyRelative ("amount").intValue == 0) {
                    EditorUtility.DisplayDialog ("Error", "Amount cannot be zero!", "Ok");
                    return;
                }
            }

            AddItemToPopUpList (itemName.stringValue);

            EditorUtility.SetDirty (itemDb);
            state = State.BLANK;
        }

        if (GUILayout.Button ("Cancel", GUILayout.Width (100))) {
            // clear the name field
            GUI.SetNextControlName ("Name");
            newItemName = string.Empty;
            GUI.FocusControl ("Name");

            itemDb.RemoveAt (selectedItem);

            newItemRequirements.Clear ();
            itemPopUpIndex.Clear ();
            newItemRequirementAmount.Clear ();

            state = State.BLANK;
        }
    }

    void RefreshDatabase () {
        // itemsDatabaseObject.ApplyModifiedProperties ();

        itemDbObject = new SerializedObject (itemDb);
        itemDbList = itemDbObject.FindProperty ("database");
        itemDb.SortAlphabeticallyAtoZ ();
    }

    List<string> GetItemUsage (string itemName) {
        var itemsInUse = new List<string> ();
        for (var i = 0; i < itemDb.Count; i++) {
            foreach (var t in itemDb.Item (i).requirements) {
                if (itemName == t.item) {
                    itemsInUse.Add (itemDb.Item (i).name);
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

    #endregion
}
#endif