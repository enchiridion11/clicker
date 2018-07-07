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
    UIBar bar;

    [SerializeField]
    CanvasGroup canvasGroup;

    [SerializeField]
    TextMeshProUGUI amountText;

    [SerializeField]
    ParticleSystem particles;

    string itemId;

    bool isCrafting;

    #endregion

    #region Properties

    public UIBar Bar {
        get { return bar; }
        set { bar = value; }
    }

    #endregion

    #region Methods

    #region Unity

    void OnDisable () {
        UnsubscribeFromEvents ();
    }

    #endregion

    public void Initialize (string itemId, int craftTime) {
        SubscribeToEvents ();

        this.itemId = itemId;
        itemImage.sprite = UIManager.Instance.GetItemIcon (this.itemId);
        bar.Initialize (0, craftTime);
    }

    void SubscribeToEvents () {
    }

    void UnsubscribeFromEvents () {
    }
    
    public void OnCraftStart (int craftTime) {
        bar.Initialize (0, craftTime);
        bar.gameObject.SetActive (true);
        isCrafting = true;
        print ("OnCraftStart" + bar.SliderValue);
    }

    public void OnCraftComplete (int amount) {
        bar.gameObject.SetActive (false);
        bar.SliderValue = 0;
        canvasGroup.alpha = 1f;
        SetAmount (amount);
        isCrafting = false;
    }

    public void SetAmount (int amount) {
        amountText.text = amount.ToString ();
        particles.Play ();
    }

    public void Sell () {
        if (!isCrafting) {
            InventoryManager.Instance.SellItem (itemId);
        }
    }

    #endregion
}