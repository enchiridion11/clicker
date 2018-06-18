using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningManager : MonoBehaviour {
    #region Fields

    [SerializeField]
    MiningResource[] resourceMines;

    #endregion

    #region Properties

    public static MiningManager Instance { get; private set; }

    #endregion

    #region Events

    public Action<string> OnResourceMined;

    #endregion

    #region Methods

    #region Unity

    void OnDisable () {
        Instance = this;
        Terminate ();
    }

    #endregion

    public void Initialize () {
        Instance = this;
        foreach (var mine in resourceMines) {
            mine.OnResourceMined += AddItemToInventory;
        }
    }

    void Terminate () {
        foreach (var mine in resourceMines) {
            mine.OnResourceMined -= AddItemToInventory;
        }
    }

    void AddItemToInventory (string resource) {
        if (OnResourceMined != null) {
            OnResourceMined (resource);
        }
    }

    #endregion
}