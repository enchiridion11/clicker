using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour {
    #region Fields

    #endregion

    #region Properties

    public int Index { get; set; }

    public InventoryItemPresenter ItemPresenter { get; set; }

    public string Item { get; set; }

    public int Amount { get; set; }

    public bool IsEmpty { get; set; }

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

    void IncreaseAmount (string name) {
        if (Item == name) {
            Amount++;
            ItemPresenter.SetAmount (Amount);
        }
    }

    public void DecreaseAmount (int amount) {
        Amount -= amount;
        ItemPresenter.SetAmount (Amount);
    }

    public void RemoveItem () {
        Destroy (ItemPresenter.gameObject);
        ItemPresenter = null;
        Item = null;
        Amount = 0;
        IsEmpty = true;
    }

    #endregion
}