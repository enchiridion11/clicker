using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour {
    #region Fields

    [Header ("Parents"), SerializeField]
    Transform windows;

    [SerializeField]
    Transform dialogs;

    [Header ("Other"), SerializeField]
    GameObject overlay;

    [Header ("Dialogs"), SerializeField]
    GameObject displayDialog;

    [SerializeField]
    GameObject sellDialog;

    #endregion

    #region Properties

    public static UIManager Instance { get; private set; }

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

    public void DisplayDialog (string title, string message, string button, UnityAction callback = null) {
        overlay.SetActive (true);
        var dialog = Instantiate (displayDialog).GetComponent<UIDisplayDialog> ();
        dialog.Initialize (title, message, button, callback);
        dialog.transform.SetParent (dialogs);
        dialog.transform.localScale = Vector3.one;
        dialog.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;
    }

    public void SellDialog (string itemId) {
        overlay.SetActive (true);
        var dialog = Instantiate (sellDialog).GetComponent<UISellDialog> ();
        dialog.Initialize (itemId);
        dialog.transform.SetParent (dialogs);
        dialog.transform.localScale = Vector3.one;
        dialog.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;
    }

    public Sprite GetItemIcon (string itemId) {
        return Resources.Load<Sprite> (string.Format ("Sprites/Items/item_{0}_01", itemId));
    }

    #endregion
}