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

    void OnEnable () {
        Instance = this;
        Initialize ();
    }

    void OnDisable () {
        Instance = this;
        Terminate ();
    }

    #endregion

    void Initialize () {
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
        Debug.Log ("Added " + resource + " to inventory.");
        if (OnResourceMined != null) {
            OnResourceMined (resource);
        }
    }

    #endregion
}