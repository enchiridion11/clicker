using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemPresenter : MonoBehaviour {
    #region Fields

    [SerializeField]
    Image itemImage;

    [SerializeField]
    TextMeshProUGUI amountText;

    [SerializeField]
    ParticleSystem particles;

    string id;

    #endregion

    #region Properties

    #endregion

    #region Methods

    #region Unity

    void OnDisable () {
        UnsubscribeFromEvents ();
    }

    #endregion

    public void Initialize (string itemId, int amount) {
        SubscribeToEvents ();

        SetAmount (amount);
        id = itemId;
        itemImage.sprite = UIManager.Instance.GetItemIcon (id);
    }

    void SubscribeToEvents () {
    }

    void UnsubscribeFromEvents () {
    }

    public void SetAmount (int amount) {
        amountText.text = amount.ToString ();
        particles.Play ();
    }

    public void Sell () {
        var dialog = UIManager.Instance.OpenDialog<UISellDialog> (UIWindowManager.Instance.SELL);
        dialog.Initialize (id);
    }

    #endregion
}