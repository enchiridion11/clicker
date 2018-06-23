using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowManager : MonoBehaviour {
    #region Fields

    #endregion

    #region Properties

    public static UIWindowManager Instance;

    public string SELL {
        get { return "UI Windows/ui_dialog_sell"; }
    }

    public string ALERT {
        get { return "UI Windows/ui_dialog"; }
    }

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize () {
        Instance = this;
    }

    public GameObject GetDialog (string dialog) {
        var go = Instantiate (Resources.Load<GameObject> (dialog));
        go.transform.SetParent (UIManager.Instance.Dialogs);
        go.transform.localScale = Vector3.one;
        go.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;
        return go;
    }

    public GameObject GetWindow (string window) {
        var go = Instantiate (Resources.Load<GameObject> (window));
        go.transform.SetParent (UIManager.Instance.Windows);
        go.transform.localScale = Vector3.one;
        go.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;
        return go;
    }

    #endregion
}