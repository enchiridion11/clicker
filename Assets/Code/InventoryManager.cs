using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    #region Fields

    [SerializeField]
    int inventorySize;

    [SerializeField]
    GameObject itemPrefab;

    [SerializeField]
    Transform slotsParent;

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

    #endregion

    #region Methods

    #region Unity

    void OnDisable () {
        UnsubscribeFromEvents ();
    }

    #endregion

    public void Initialize () {
        Instance = this;

        SubscribeToEvents ();

        for (var i = 0; i < slotsParent.childCount; i++) {
            slots.Add (slotsParent.GetChild (i).GetComponent<InventorySlot> ());
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
                    item.Initialize (name);
                    item.transform.SetParent (slots[i].transform);
                    item.transform.localScale = Vector3.one;
                    item.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;

                    // add item to list
                    slots[i].Item = item;
                    print (slots[i].Item.Amount);

                    slots[i].Item = item;
                    slots[i].IsEmpty = false;
                    break;
                }
            }
        }

        if (OnAddMinedResource != null) {
            OnAddMinedResource (name);
        }
    }

    public bool HasItem (string name) {
        for (var i = 0; i < slots.Count; i++) {
            if (!slots[i].IsEmpty) {
                if (slots[i].Item.Name == name) {
                    return true;
                }
            }
        }

        return false;
    }

    public int GetItemAmount (string name) {
        for (var i = 0; i < slots.Count; i++) {
            if (!slots[i].IsEmpty) {
                if (slots[i].Item.Name == name) {
                    return slots[i].Item.Amount;
                }
            }
        }

        return 0;
    }

    #endregion
}