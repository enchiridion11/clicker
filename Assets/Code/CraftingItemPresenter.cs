using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    List<ItemRequirements> requirements = new List<ItemRequirements> ();

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
        InventoryManager.Instance.OnAddItem += CheckRequirements;
        InventoryManager.Instance.OnRemoveItem += CheckRequirements;
    }

    void UnsubscribeFromEvents () {
        InventoryManager.Instance.OnAddItem -= CheckRequirements;
        InventoryManager.Instance.OnRemoveItem -= CheckRequirements;
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

        // check if inventory amount satisfies each requirement
        foreach (var requirement in requirements) {
            if (requirement.item != itemId) {
                continue;
            }

            if (amount < requirement.amount) {
                requirement.canCraft = false;
                break;
            }

            requirement.canCraft = true;
        }

        canCraft = CanCraft ();
        CalculateCraftingAmount ();
    }

    bool CanCraft () {
        foreach (var requirement in requirements) {
            if (!requirement.canCraft) {
                return false;
            }
        }

        return true;
    }

    bool RequirementHasItem (string itemId) {
        foreach (var requirement in requirements) {
            if (requirement.item == itemId) {
                return true;
            }
        }

        return false;
    }

    void CalculateCraftingAmount () {
        var craftAmount = new List<int> ();
        if (canCraft) {
            foreach (var requirement in requirements) {
                craftAmount.Add (InventoryManager.Instance.GetItemAmount (requirement.item) / requirement.amount);
            }

            canvasGroup.alpha = 1f;
            SetAmount (craftAmount.Min());
        }
        else {
            canvasGroup.alpha = 0.4f;
            SetAmount (0);
        }
    }

    public void Craft () {
        if (canCraft) {
            InventoryManager.Instance.RemoveItems (requirements);
            InventoryManager.Instance.AddItem (itemId);
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