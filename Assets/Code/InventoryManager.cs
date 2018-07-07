using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour {
    #region Fields

    [SerializeField]
    GameObject titleText;

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

    public Action<string, int> OnAddItem;
    public Action<string, int> OnRemoveItem;

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
        MiningManager.Instance.OnResourceMined += AddItem;
    }

    void UnsubscribeFromEvents () {
        MiningManager.Instance.OnResourceMined -= AddItem;
    }

    public void AddItem (string itemId) {
        if (!HasItem (itemId)) {
            for (var i = 0; i < slots.Count; i++) {
                if (slots[i].IsEmpty) {
                    var item = Instantiate (itemPrefab).GetComponent<InventoryItemPresenter> ();
                    item.Initialize (itemId, 0);
                    item.transform.SetParent (slots[i].transform);
                    item.transform.localScale = Vector3.one;
                    item.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;

                    // add item to list
                    slots[i].ItemPresenter = item;
                    slots[i].ItemId = itemId;
                    slots[i].Amount = 0;
                    slots[i].IsEmpty = false;
                    break;
                }
            }
        }

        if (OnAddItem != null) {
            OnAddItem (itemId, GetItemAmount (itemId) + 1);
        }

        // temp victory condition
        if (itemId == "violin") {
            ShowVictoryScreen ();
        }
    }

    public void RemoveItems (List<ItemRequirements> items) {
        foreach (var item in items) {
            var slot = GetItemSlot (item.item);

            slot.DecreaseAmount (item.amount);

            if (OnRemoveItem != null) {
                OnRemoveItem (item.item, GetItemAmount (item.item));
            }
        }
    }

    public bool HasItem (string itemId) {
        for (var i = 0; i < slots.Count; i++) {
            if (!slots[i].IsEmpty) {
                if (slots[i].ItemId == itemId) {
                    return true;
                }
            }
        }

        return false;
    }

    public int GetItemAmount (string itemId) {
        for (var i = 0; i < slots.Count; i++) {
            if (!slots[i].IsEmpty) {
                if (slots[i].ItemId == itemId) {
                    return slots[i].Amount;
                }
            }
        }

        return 0;
    }

    InventorySlot GetItemSlot (string itemId) {
        for (var i = 0; i < slots.Count; i++) {
            if (!slots[i].IsEmpty) {
                if (slots[i].ItemId == itemId) {
                    return slots[i];
                }
            }
        }

        return null;
    }

    int GetItemSlotIndex (string itemId) {
        for (var i = 0; i < slots.Count; i++) {
            if (!slots[i].IsEmpty) {
                if (slots[i].ItemId == itemId) {
                    return i;
                }
            }
        }

        return -1;
    }

    public void SellItem (string itemId) {
        var dialog = UIManager.Instance.OpenDialog<UISellDialog> (UIWindowManager.SELL, UIManager.Instance.Dialogs);
        var sellAmount = Data.GetItemData (itemId).SellAmount;
        dialog.Initialize (itemId, sellAmount);
    }

    public void OnSellItem (string itemId) {
        var sellAmount = Data.GetItemData (itemId).SellAmount;
        var slot = GetItemSlot (itemId);

        UIManager.Instance.SellItemAnimation (slot.transform.position);

        CurrencyManager.Instance.IncreaseCurrency ("gold", sellAmount);

        //TODO: add amount to dialog
        slot.DecreaseAmount (1);
    }

    void ShowVictoryScreen () {
        var dialog = UIManager.Instance.OpenDialog<UIAlertDialog> (UIWindowManager.ALERT, UIManager.Instance.Dialogs);
        dialog.Initialize ("you win", "now go fiddle your fiddle", "fiddle!", RestartGame);
    }

    void RestartGame () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    #endregion
}