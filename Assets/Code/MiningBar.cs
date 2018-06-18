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

    int currentTaps = 1;
    int maxTaps;

    #endregion

    #region Properties

    #endregion
    
    #region Events

    public Action OnMaxTaps;

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize (int max) {
        maxTaps = max;
        slider.value = 0;
        slider.maxValue = max;
        currentTapsText.text = currentTaps.ToString ();
        maxTapsText.text = maxTaps.ToString ();
    }

    public void Increase () {
        if (currentTaps < maxTaps) {
            slider.value = currentTaps;
            currentTapsText.text = currentTaps.ToString ();
            currentTaps++;
        }
        else {
            currentTaps = 1;
            gameObject.SetActive (false);

            if (OnMaxTaps != null) {
                OnMaxTaps();
            }
        }
    }

    #endregion
}