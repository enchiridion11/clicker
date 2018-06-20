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
    
    public CraftingItemPresenter Item { get; private set; }

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize () {
        Item = Instantiate (itemPrefab).GetComponent<CraftingItemPresenter> ();
        Item.Initialize (name);
        Item.transform.SetParent (transform);
        Item.transform.localScale = Vector3.one;
        Item.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;
    }

    #endregion
}