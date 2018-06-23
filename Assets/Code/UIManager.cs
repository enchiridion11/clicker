using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour {
    #region Fields

    [Header ("Parents"), SerializeField]
    Transform windows;

    [SerializeField]
    Transform dialogs;

    [SerializeField]
    GameObject overlay;

    #endregion

    #region Properties

    public static UIManager Instance { get; private set; }

    public Transform Windows {
        get { return windows; }
    }

    public Transform Dialogs {
        get { return dialogs; }
    }

    public GameObject Overlay {
        get { return overlay; }
    }

    #endregion

    #region Events

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize () {
        Instance = this;
    }

    public T OpenDialog<T> (string dialog) {
        overlay.SetActive (true);
        return UIWindowManager.Instance.GetDialog (dialog).GetComponent<T> ();
    }

    public T OpenWindow<T> (string window) {
        return UIWindowManager.Instance.GetWindow (window).GetComponent<T> ();
    }

    public Sprite GetItemIcon (string itemId) {
        return Resources.Load<Sprite> (string.Format ("Sprites/Items/item_{0}_01", itemId));
    }

    #endregion
}