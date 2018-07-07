using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISellDialog : MonoBehaviour {
    #region Fields

    [SerializeField]
    UISpinner spinner;

    [SerializeField]
    Image itemImage;

    [SerializeField]
    TextMeshProUGUI messageText;

    string itemId;

    int itemValue;
    int sellAmount = 1;

    #endregion

    #region Properties

    #endregion

    #region Events

    #endregion

    #region Methods

    #region Unity

    #endregion

    void OnEnable () {
        spinner.OnValueChange += OnSellAmountChange;
    }

    void OnDisable () {
        spinner.OnValueChange -= OnSellAmountChange;
    }

    public void Initialize (string itemId, int sellValue) {
        this.itemId = itemId;
        itemValue = sellValue;
        spinner.Initialize (1, 1, InventoryManager.Instance.GetItemAmount (itemId));
        itemImage.sprite = UIManager.Instance.GetItemIcon (itemId);
        messageText.text = string.Format ("for +<color=#A7E22E>{0}</color>", sellValue);
    }

    void OnSellAmountChange (int amount) {
        sellAmount = itemValue * amount;
        messageText.text = string.Format ("for +<color=#A7E22E>{0}</color>", sellAmount);
    }

    public void Sell () {
        InventoryManager.Instance.OnSellItem (itemId, sellAmount);
        Close ();
    }

    public void Close () {
        UIManager.Instance.Overlay.SetActive (false);
        Destroy (gameObject);
    }

    #endregion
}