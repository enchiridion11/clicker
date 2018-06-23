using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowManager : MonoBehaviour {
    #region Fields

    #endregion

    #region Properties

    public static UIWindowManager Instance;

    public static string SELL {
        get { return "UI Windows/ui_dialog_sell"; }
    }

    public static string ALERT {
        get { return "UI Windows/ui_dialog_alert"; }
    }
    
    public static string CONFIRM {
        get { return "UI Windows/ui_dialog_confirm"; }
    }
    
    public static string PROMPT {
        get { return "UI Windows/ui_dialog_prompt"; }
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