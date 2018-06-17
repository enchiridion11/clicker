using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    #region Fields

    [SerializeField]
    int inventorySize;

    [SerializeField]
    GameObject itemPrefab;

    [SerializeField]
    Transform slotsParent;

    List<InventorySlot> slots = new List<InventorySlot> ();

    #endregion

    #region Properties

    public static InventoryManager Instance { get; private set; }

    #endregion

    #region Methods

    #region Unity

    void OnEnable () {
        Instance = this;
    }

    void OnDisable () {
        MiningManager.Instance.OnResourceMined -= AddMinedResource;
    }

    #endregion

    public void Initialize () {
        MiningManager.Instance.OnResourceMined += AddMinedResource;

        for (var i = 0; i < slotsParent.childCount; i++) {
            slots.Add (slotsParent.GetChild (i).GetComponent<InventorySlot> ());
        }
    }

    void AddMinedResource (string name) {
        if (slots[0].IsEmpty) {
            var item = Instantiate (itemPrefab).GetComponent<InventoryItemPresenter> ();
            item.Initialize (name);
            item.transform.SetParent (slots[0].transform);
            item.transform.localScale = Vector3.one;
            item.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;

            slots[0].Item = item;
            slots[0].IsEmpty = false;
            return;
        }

        slots[0].Item.IncreaseAmount ();
    }

    #endregion
}