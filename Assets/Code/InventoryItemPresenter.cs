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

    string name;

    int amount = 1;

    #endregion

    #region Properties

    public string Name {
        get { return name; }
    }

    public int Amount {
        get { return amount; }
    }

    #endregion

    #region Methods

    #region Unity

    void OnEnable () {
        InventoryManager.Instance.OnAddMinedResource += IncreaseAmount;
    }

    void OnDisable () {
        InventoryManager.Instance.OnAddMinedResource -= IncreaseAmount;
    }

    #endregion

    public void Initialize (string item) {
        name = item;
        itemImage.sprite = GetItemIcon (item);
    }

    public void IncreaseAmount (string item) {
        if (item == name) {
            amountText.text = amount.ToString ();
            amount++;
            particles.Play ();
        }
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