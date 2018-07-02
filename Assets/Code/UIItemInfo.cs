using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItemInfo : MonoBehaviour {
    #region Fields

    [SerializeField]
    TextMeshProUGUI titleText;

    [SerializeField]
    TextMeshProUGUI descriptionText;

    [SerializeField]
    TextMeshProUGUI timeText;

    [SerializeField]
    GameObject requirementPrefab;

    [SerializeField]
    Transform infoParent;

    #endregion

    #region Properties

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize (string itemId) {
        var item = Data.GetItemData (itemId);
        titleText.text = itemId;
        foreach (var requirement in item.Requirements) {
            var req = Instantiate (requirementPrefab).GetComponent<UIItemRequirement> ();
            req.transform.SetParent (infoParent);
            req.transform.localScale = Vector3.one;
            req.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;
            req.Initialize (requirement.item, requirement.amount);
        }
    }

    public void Close () {
        Destroy (gameObject);
    }

    #endregion
}