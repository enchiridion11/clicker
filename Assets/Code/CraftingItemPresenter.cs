using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItemPresenter : MonoBehaviour {
    #region Fields

    [SerializeField]
    Image itemImage;

    [SerializeField]
    TextMeshProUGUI amountText;

    [SerializeField]
    CanvasGroup canvasGroup;

    string id;

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

        id = itemId;
        itemImage.sprite = UIManager.Instance.GetItemIcon (itemId);
        amountText.text = amount.ToString ();
    }

    void SubscribeToEvents () {
        InventoryManager.Instance.OnAddMinedResource += CheckRequirements;
    }

    void UnsubscribeFromEvents () {
        InventoryManager.Instance.OnAddMinedResource -= CheckRequirements;
    }

    public void SetAmount (int value) {
        amount = value;
        amountText.text = amount.ToString ();
    }

    void CheckRequirements (string resource) {
        requirements = Data.GetItemData (id).GetItemRequirements (id);
        if (requirements.Count > 0) {
            var index = 0;
            print ("requirements.Count: " + requirements.Count);
            // check if requirements exist in inventory and if there are enough
            foreach (var requirement in requirements) {
                print ("requirement.Key: " + requirement.Key + ", resource: " + resource);

                if (InventoryManager.Instance.HasItem (requirement.Key)) {
                    print ("contains: " + requirement.Key);
                    print ("amount: " + InventoryManager.Instance.GetItemAmount (requirement.Key));
                    if (InventoryManager.Instance.GetItemAmount (requirement.Key) < requirement.Value) {
                        print ("requirement.Key amount: " + InventoryManager.Instance.GetItemAmount (requirement.Key) + ", requirement.Value:" + requirement.Value);
                        canCraft = false;
                        CalculateCraftingAmount (requirements);
                        return;
                    }
                }
                else {
                    canCraft = false;
                    CalculateCraftingAmount (requirements);
                    return;
                }

                index++;
            }

            print ("requirements met");
            // requirements met, can now set amount
            canCraft = true;
            CalculateCraftingAmount (requirements);
        }
        else {
            GetComponent<CanvasGroup> ().alpha = 1f;
        }
    }

    void CalculateCraftingAmount (Dictionary<string, int> requirements) {
        var amount = 0;
        foreach (var requirement in requirements) {
            amount = InventoryManager.Instance.GetItemAmount (requirement.Key) / requirement.Value;
        }

        GetComponent<CanvasGroup> ().alpha = canCraft ? 1f : 0.4f;
        SetAmount (amount);
    }

    public void Craft () {
        if (canCraft) {
            InventoryManager.Instance.RemoveRequirements (requirements);
            InventoryManager.Instance.AddCraftedItem (id);
            CheckRequirements (id);
        }
        else {
            UIManager.Instance.DisplayDialog ("warning", "requirements not met!", "ok");
        }
    }

    #endregion
}