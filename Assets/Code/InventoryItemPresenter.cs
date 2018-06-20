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

    public void Initialize (string item, int amount) {
        SubscribeToEvents ();

        SetAmount (amount);
        itemImage.sprite = GetItemIcon (item);
    }

    void SubscribeToEvents () {
       // InventoryManager.Instance.OnAddMinedResource += IncreaseAmount;
       // InventoryManager.Instance.OnAddCraftedItem += IncreaseAmount;
    }

    void UnsubscribeFromEvents () {
        //InventoryManager.Instance.OnAddMinedResource -= IncreaseAmount;
       // InventoryManager.Instance.OnAddCraftedItem -= IncreaseAmount;
    }

    public void SetAmount (int amount) {
            amountText.text = amount.ToString ();
            particles.Play ();
       }

    Sprite GetItemIcon (string item) {
        return Resources.Load<Sprite> (string.Format ("Sprites/Items/item_{0}_01", item));
    }

    #endregion
}