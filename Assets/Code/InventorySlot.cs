using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour {
    #region Fields

    #endregion

    #region Properties

    public InventoryItemPresenter ItemPresenter { get; set; }

    public string ItemId { get; set; }

    public int Amount { get; set; }

    public bool IsEmpty { get; set; }

    #endregion

    #region Events

    #endregion

    #region Methods

    #region Unity

    void OnDisable () {
        UnsubscribeFromEvents ();
    }

    #endregion

    public void Initialize () {
        SubscribeToEvents ();

        IsEmpty = true;
    }

    void SubscribeToEvents () {
        InventoryManager.Instance.OnAddMinedResource += IncreaseAmount;
        InventoryManager.Instance.OnAddCraftedItem += IncreaseAmount;
    }

    void UnsubscribeFromEvents () {
        InventoryManager.Instance.OnAddMinedResource -= IncreaseAmount;
        InventoryManager.Instance.OnAddCraftedItem -= IncreaseAmount;
    }

    void IncreaseAmount (string itemId) {
        if (ItemId == itemId) {
            Amount++;
            ItemPresenter.SetAmount (Amount);
        }
    }

    public void DecreaseAmount (int amount) {
        Amount -= amount;
        if (Amount > 0) {
            ItemPresenter.SetAmount (Amount);
        }
        else {
            RemoveItem ();
        }
    }

    public void RemoveItem () {
        Destroy (ItemPresenter.gameObject);
        ItemPresenter = null;
        ItemId = null;
        Amount = 0;
        IsEmpty = true;
    }

    #endregion
}