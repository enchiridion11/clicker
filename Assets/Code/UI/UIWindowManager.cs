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

    public static string ITEM_INFO {
        get { return "UI Windows/ui_dialog_itemInfo"; }
    }

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize () {
        Instance = this;
    }

    public GameObject GetDialog (string dialog) {
        return Instantiate (Resources.Load<GameObject> (dialog));
    }

    public GameObject GetWindow (string window) {
        return Instantiate (Resources.Load<GameObject> (window));
    }

    #endregion
}