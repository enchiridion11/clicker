using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiningBar : MonoBehaviour {
    #region Fields

    [SerializeField]
    Slider slider;

    [SerializeField]
    TextMeshProUGUI currentTapsText;

    [SerializeField]
    TextMeshProUGUI maxTapsText;

    #endregion

    #region Properties

    #endregion

    #region Events

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize (int current, int max) {
        slider.value = current;
        slider.maxValue = max;
        currentTapsText.text = current.ToString ();
        maxTapsText.text = max.ToString ();
    }

    #endregion
}