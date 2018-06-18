using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    string name;

    int amount;

    #endregion

    #region Properties

    #endregion

    #region Methods

    #region Unity

    void OnEnable () {
        InventoryManager.Instance.OnAddMinedResource += OnResourceChange;
    }

    void OnDisable () {
        InventoryManager.Instance.OnAddMinedResource -= OnResourceChange;
    }

    #endregion

    public void Initialize (string item) {
        name = item;
        itemImage.sprite = GetItemIcon (item);
        amountText.text = amount.ToString ();
    }

    public void IncreaseAmount () {
        amount++;
        amountText.text = amount.ToString ();
    }

    public void DecreaseAmount () {
        amount--;
        amountText.text = amount.ToString ();
    }

    void OnResourceChange (string resource) {
        var requirements = Data.GetItemData (name).GetItemRequirements (name);
        if (requirements.Count > 0) {
            foreach (var requirement in requirements) {
                if (InventoryManager.Instance.HasItem (requirement.Key)) {
                    print ("contains: " + requirement.Key);
                    print ("amount: " + InventoryManager.Instance.GetItemAmount (requirement.Key));
                    if (InventoryManager.Instance.GetItemAmount (requirement.Key) >= requirement.Value) {
                        GetComponent<CanvasGroup> ().alpha = 1f;
                        IncreaseAmount ();
                    }
                    else {
                        GetComponent<CanvasGroup> ().alpha = 0.4f;
                    }
                }
                else {
                    GetComponent<CanvasGroup> ().alpha = 0.4f;
                }
            }
        }
        else {
            GetComponent<CanvasGroup> ().alpha = 1f;
        }
    }

    Sprite GetItemIcon (string item) {
        return Resources.Load<Sprite> (string.Format ("Sprites/Items/item_{0}_01", item));
    }

    #endregion
}