using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIAlertDialog : MonoBehaviour {
    #region Fields

    [SerializeField]
    TextMeshProUGUI titleText;

    [SerializeField]
    TextMeshProUGUI messageText;

    [SerializeField]
    TextMeshProUGUI buttonText;

    [SerializeField]
    UIButton button;

    #endregion

    #region Properties

    #endregion

    #region Events

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize (string title, string message, string buttonTitle, UnityAction callback = null) {
        titleText.text = title;
        messageText.text = message;
        buttonText.text = buttonTitle;
        button.OnClick.AddListener (callback ?? Close);
    }

    public void Close () {
        UIManager.Instance.Overlay.SetActive (false);
        Destroy (gameObject);
    }

    #endregion
}