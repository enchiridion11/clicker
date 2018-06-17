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

    int amount;

    #endregion

    #region Properties

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize (string item) {
        itemImage.sprite = GetItemIcon (item);
        IncreaseAmount ();
    }

    public void IncreaseAmount () {
        amount++;
        amountText.text = amount.ToString ();
        particles.Play();
    }

    public void DecreaseAmount () {
        amount--;
        amountText.text = amount.ToString ();
    }

    Sprite GetItemIcon (string item) {
        return Resources.Load<Sprite> (string.Format ("Sprites/Items/item_{0}_01", item));
    }

    #endregion
}