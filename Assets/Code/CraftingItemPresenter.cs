using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingItemPresenter : MonoBehaviour {
    #region Fields

    [SerializeField]
    Image itemImage;

    [SerializeField]
    TextMeshProUGUI amountText;

    [SerializeField]
    CanvasGroup canvasGroup;

    string itemId;

    int amount;

    bool canCraft;

    Dictionary<string, int> requirements = new Dictionary<string, int> ();

    #endregion

    #region Properties

    #endregion

    #region Events

    #endregion

    #region Methods

    #region Unity

    void OnDisable () {
        UnsubscribeFromEvents ();
    }

    #endregion

    public void Initialize (string itemId) {
        SubscribeToEvents ();

        this.itemId = itemId;
        itemImage.sprite = UIManager.Instance.GetItemIcon (itemId);
        requirements = Data.GetItemData (itemId).GetItemRequirements (itemId);

        amountText.text = amount.ToString ();
    }

    void SubscribeToEvents () {
        InventoryManager.Instance.OnItemChange += CheckRequirements;
    }

    void UnsubscribeFromEvents () {
        InventoryManager.Instance.OnItemChange -= CheckRequirements;
    }

    public void SetAmount (int value) {
        amount = value;
        amountText.text = amount.ToString ();
    }

    void CheckRequirements (string itemId, int amount) {
        // if item added to inventory is not in requirements, do nothing
        if (!RequirementHasItem (itemId)) {
            return;
        }

        // check if requirements exist in inventory and if there are enough
        foreach (var requirement in requirements) {
            print ("amount: " + amount);
            if (amount < requirement.Value) {
                print ("requirements NOT met");
                canCraft = false;
                CalculateCraftingAmount ();
                return;
            }
        }

        print ("requirements met");
        // requirements met, can now set amount
        canCraft = true;
        CalculateCraftingAmount ();
    }

    bool RequirementHasItem (string itemId) {
        foreach (var requirement in requirements) {
            if (requirement.Key == itemId) {
                return true;
            }
        }

        return false;
    }

    void CalculateCraftingAmount () {
        var amount = 0;
        foreach (var requirement in requirements) {
            amount = InventoryManager.Instance.GetItemAmount (requirement.Key) / requirement.Value;
        }

        canvasGroup.alpha = canCraft ? 1f : 0.4f;
        SetAmount (amount);
    }

    public void Craft () {
        if (canCraft) {
            InventoryManager.Instance.RemoveItem (requirements);
            InventoryManager.Instance.AddCraftedItem (itemId);
        }
        else {
            var dialog = UIManager.Instance.OpenDialog<UIAlertDialog> (UIWindowManager.ALERT, UIManager.Instance.Dialogs);
            dialog.Initialize ("warning", "requirements not met!", "ok");
        }
    }

    public void ShowItemInfoDialog () {
        var dialog = UIManager.Instance.OpenDialog<UIItemInfo> (UIWindowManager.ITEM_INFO, UIManager.Instance.Dialogs, false);
        var button = GetComponent<UIButtonHold> ();
        var canvas = UIManager.Instance.Canvas;
        var offset = new Vector2 (0, 40);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle (canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);

        dialog.GetComponent<RectTransform> ().localPosition = pos + offset;
        dialog.Initialize (itemId);
        button.OnClick.AddListener (dialog.Close);
        button.OnRelease.AddListener (dialog.Close);
    }

    #endregion
}