using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour {
    #region Fields

    [SerializeField]
    bool isEmpty = true;

    InventoryItemPresenter item;

    #endregion

    #region Properties

    public bool IsEmpty {
        get { return isEmpty; }
        set { isEmpty = value; }
    }

    public InventoryItemPresenter Item { get; set; }

    #endregion

    #region Methods

    #region Unity

    #endregion

    #endregion
}