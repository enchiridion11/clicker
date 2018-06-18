using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour {
    #region Fields

    [SerializeField]
    Transform slotParent;

    #endregion

    #region Properties

    public static CraftingManager Instance { get; private set; }

    #endregion

    #region Events

    public Action OnInitialize;

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize () {
        Instance = this;
        foreach (var slot in slotParent.GetComponentsInChildren<CraftingSlot> ()) {
            slot.Initialize ();
        }
    }

    #endregion
}