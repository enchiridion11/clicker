using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISpinner : MonoBehaviour {
    #region Fields

    [SerializeField]
    int minAmount;

    [SerializeField]
    int maxAmount = 1;

    [SerializeField]
    int step = 1;

    [SerializeField]
    TextMeshProUGUI amountText;

    int amount;

    #endregion

    #region Properties

    #endregion

    #region Events

    public Action<int> OnValueChange;

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize (int startingValue, int minValue, int maxValue) {
        amount = startingValue;
        minAmount = minValue;
        maxAmount = maxValue;
        amountText.text = amount.ToString ();
    }

    public void Increase () {
        amount += step;
        amount = Mathf.Clamp (amount, minAmount, maxAmount);
        amountText.text = amount.ToString ();
        if (OnValueChange != null) {
            OnValueChange (amount);
        }
    }

    public void Decrease () {
        amount -= step;
        amount = Mathf.Clamp (amount, minAmount, maxAmount);
        amountText.text = amount.ToString ();
        if (OnValueChange != null) {
            OnValueChange (amount);
        }
    }

    #endregion
}