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
        itemImage.sprite = UIManager.Instance.GetItemIcon (itemId);
    }

    void SubscribeToEvents () {
    }

    void UnsubscribeFromEvents () {
    }

    public void SetAmount (int amount) {
        amountText.text = amount.ToString ();
        particles.Play ();
    }

    #endregion
}