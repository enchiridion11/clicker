using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSlot : MonoBehaviour {
    #region Fields

    [SerializeField]
    string name;

    [SerializeField]
    GameObject itemPrefab;

    #endregion

    #region Properties

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize () {
        var item = Instantiate (itemPrefab).GetComponent<CraftingItemPresenter> ();
        item.Initialize (name);
        item.transform.SetParent (transform);
        item.transform.localScale = Vector3.one;
        item.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;
    }

    #endregion
}