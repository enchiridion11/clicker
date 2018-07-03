using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemRequirement : MonoBehaviour {
    #region Fields

    [SerializeField]
    Image itemImage;

    [SerializeField]
    TextMeshProUGUI amountText;

    [SerializeField]
    Color normalTextColor;

    [SerializeField]
    Color insufficientTextColor;

    #endregion

    #region Properties

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize (string itemId, int amount, bool requirementsMet) {
        itemImage.sprite = UIManager.Instance.GetItemIcon (itemId);
        amountText.text = amount.ToString ();
        amountText.color = requirementsMet ? normalTextColor : insufficientTextColor;
    }

    #endregion
}