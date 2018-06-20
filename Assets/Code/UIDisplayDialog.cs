using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIDisplayDialog : MonoBehaviour {
    #region Fields

    [SerializeField]
    TextMeshProUGUI titleText;

    [SerializeField]
    TextMeshProUGUI messageText;

    [SerializeField]
    TextMeshProUGUI buttonText;

    [SerializeField]
    Button button;

    #endregion

    #region Properties

    #endregion

    #region Events

    public Action<UIDisplayDialog> OnClose;

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize (string title, string message, string buttonTitle, UnityAction callback = null) {
        titleText.text = title;
        messageText.text = message;
        buttonText.text = buttonTitle;
        button.onClick.AddListener (callback ?? Close);
    }

    public void Close () {
        if (OnClose != null) {
            OnClose (this);
        }

        Destroy (gameObject);
    }

    #endregion
}