using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour {
    #region Fields

    [SerializeField]
    Canvas canvas;

    [Header ("Parents"), SerializeField]
    Transform windows;

    [SerializeField]
    Transform dialogs;

    [SerializeField]
    GameObject overlay;

    #endregion

    #region Properties

    public static UIManager Instance { get; private set; }

    public Canvas Canvas {
        get { return canvas; }
    }

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

    public T OpenDialog<T> (string dialog, Transform parent, bool showOverlay = true) {
        overlay.SetActive (showOverlay);
        var go = UIWindowManager.Instance.GetDialog (dialog);
        go.transform.SetParent (parent);
        go.transform.localScale = Vector3.one;
        go.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;
        return go.GetComponent<T> ();
    }

    public T OpenWindow<T> (string window, Transform parent) {
        var go = UIWindowManager.Instance.GetDialog (window);
        go.transform.SetParent (parent);
        go.transform.localScale = Vector3.one;
        go.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;
        return go.GetComponent<T> ();
    }

    public Sprite GetItemIcon (string itemId) {
        return Resources.Load<Sprite> (string.Format ("Sprites/Items/item_{0}_01", itemId));
    }

    #endregion
}