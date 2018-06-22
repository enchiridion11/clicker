using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour {
    #region Fields

    [SerializeField]
    GameObject titleText;

    [SerializeField]
    Transform slotParent;

    List<CraftingSlot> slots = new List<CraftingSlot> ();

    #endregion

    #region Properties

    public static CraftingManager Instance { get; private set; }

    public List<CraftingSlot> Slots {
        get { return slots; }
    }

    #endregion

    #region Events

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize () {
        Instance = this;
        titleText.SetActive (true);
        foreach (var slot in slotParent.GetComponentsInChildren<CraftingSlot> ()) {
            slots.Add (slot);
            slot.Initialize ();
        }
    }

    #endregion
}