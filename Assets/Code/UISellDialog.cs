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
    TextMeshProUGUI messageText;

    #endregion

    #region Properties

    #endregion

    #region Events

    public Action<UISellDialog> OnClose;

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize (string itemId) {
        var sellAmount = Data.GetItemData (itemId).Clicks;
        messageText.text = string.Format ("Sell {0} for <color=#F92772>{1}</color> gold?", itemId, sellAmount);
    }

    public void Sell () {
        Close ();
    }

    void Close () {
        UIManager.Instance.Overlay.SetActive (false);
        Destroy (gameObject);
    }

    #endregion
}