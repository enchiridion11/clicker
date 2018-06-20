using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour {
    #region Fields

    [SerializeField]
    int inventorySize;

    [SerializeField]
    GameObject itemPrefab;

    [SerializeField]
    Transform slotsParent;

    [SerializeField]
    GameObject titleText;

    List<InventorySlot> slots = new List<InventorySlot> ();

    #endregion

    #region Properties

    public static InventoryManager Instance { get; private set; }

    public List<InventorySlot> Slots {
        get { return slots; }
    }

    #endregion

    #region Events

    public Action<string> OnAddMinedResource;
    public Action<string> OnAddCraftedItem;

    #endregion

    #region Methods

    #region Unity

    void OnDisable () {
        UnsubscribeFromEvents ();
    }

    #endregion

    public void Initialize () {
        Instance = this;
        titleText.SetActive (true);
        SubscribeToEvents ();

        for (var i = 0; i < slotsParent.childCount; i++) {
            var slot = slotsParent.GetChild (i).GetComponent<InventorySlot> ();
            slot.Initialize ();
            slots.Add (slot);
        }
    }

    void SubscribeToEvents () {
        MiningManager.Instance.OnResourceMined += AddMinedResource;
    }

    void UnsubscribeFromEvents () {
        MiningManager.Instance.OnResourceMined -= AddMinedResource;
    }

    void AddMinedResource (string name) {
        if (!HasItem (name)) {
            for (var i = 0; i < slots.Count; i++) {
                if (slots[i].IsEmpty) {
                    var item = Instantiate (itemPrefab).GetComponent<InventoryItemPresenter> ();
                    item.Initialize (name, 0);
                    item.transform.SetParent (slots[i].transform);
                    item.transform.localScale = Vector3.one;
                    item.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;

                    // add item to list
                    slots[i].ItemPresenter = item;
                    slots[i].Item = name;
                    slots[i].Amount = 0;
                    slots[i].IsEmpty = false;
                    break;
                }
            }
        }

        if (OnAddMinedResource != null) {
            OnAddMinedResource (name);
        }
    }

    public void AddCraftedItem (string name) {
        if (!HasItem (name)) {
            for (var i = 0; i < slots.Count; i++) {
                if (slots[i].IsEmpty) {
                    var item = Instantiate (itemPrefab).GetComponent<InventoryItemPresenter> ();
                    item.Initialize (name, 0);
                    item.transform.SetParent (slots[i].transform);
                    item.transform.localScale = Vector3.one;
                    item.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;

                    // add item to list
                    slots[i].ItemPresenter = item;
                    slots[i].Item = name;
                    slots[i].Amount = 0;
                    slots[i].IsEmpty = false;
                    break;
                }
            }
        }

        if (OnAddMinedResource != null) {
            OnAddMinedResource (name);
        }
        
                
        if (name == "Violin") {
            ShowVictoryScreen ();
        }
    }

    public void RemoveRequirements (Dictionary<string, int> items) {
        foreach (var item in items) {
            var slot = GetItemSlot (item.Key);

            slot.DecreaseAmount (item.Value);

            if (slot.Amount <= 0) {
                slot.RemoveItem ();
            }
        }
    }

    public bool HasItem (string name) {
        for (var i = 0; i < slots.Count; i++) {
            if (!slots[i].IsEmpty) {
                if (slots[i].Item == name) {
                    return true;
                }
            }
        }

        return false;
    }

    public int GetItemAmount (string name) {
        for (var i = 0; i < slots.Count; i++) {
            if (!slots[i].IsEmpty) {
                if (slots[i].Item == name) {
                    return slots[i].Amount;
                }
            }
        }

        return 0;
    }

    InventorySlot GetItemSlot (string name) {
        for (var i = 0; i < slots.Count; i++) {
            if (!slots[i].IsEmpty) {
                if (slots[i].Item == name) {
                    return slots[i];
                }
            }
        }

        return null;
    }

    int GetItemSlotIndex (string name) {
        for (var i = 0; i < slots.Count; i++) {
            if (!slots[i].IsEmpty) {
                if (slots[i].Item == name) {
                    return i;
                }
            }
        }

        return -1;
    }

    void ShowVictoryScreen () {
        UIManager.Instance.DisplayDialog ("you win", "now go fiddle your fiddle", "fiddle!", RestartGame);
    }

    void RestartGame () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    #endregion
}