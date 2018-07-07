using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour {
    #region Fields

    int craftTime = 0;
    int progress;

    bool isCrafting;

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
        InventoryManager.Instance.OnAddItem += StartCrafting;
        TimeManager.Instance.OnTick += OnTimeChange;
    }

    void UnsubscribeFromEvents () {
        InventoryManager.Instance.OnAddItem -= StartCrafting;
        TimeManager.Instance.OnTick -= OnTimeChange;
    }

    void StartCrafting (string itemId, int currentAmount) {
        if (ItemId == itemId) {
            craftTime = Data.GetItemData (itemId).CraftTime;
            ItemPresenter.OnCraftStart (craftTime);
            if (craftTime == 0) {
                if (craftTime == 0) {
                    isCrafting = false;
                    progress = 0;
                    Amount++;
                    ItemPresenter.OnCraftComplete (Amount);
                    return;
                }
            }

            isCrafting = true;
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

    void OnTimeChange (int tick) {
        if (!isCrafting) {
            return;
        }

        print ("progress" + progress);
        print ("OnTimeChange" + ItemPresenter.Bar.SliderValue);

        if (progress <= craftTime) {
            progress++;
            ItemPresenter.Bar.OnUpdateValue (progress);

            if (progress > craftTime) {
                isCrafting = false;
                progress = 0;
                Amount++;
                ItemPresenter.OnCraftComplete (Amount);
            }
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