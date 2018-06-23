using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour {
    #region Fields

    [SerializeField]
    TextMeshProUGUI goldText;

    [SerializeField]
    TextMeshProUGUI diamondText;

    int currencyAmount;
    int premiumCurrencyAmount;

    #endregion

    #region Properties

    public static CurrencyManager Instance { get; private set; }

    CurrencyDefaults[] Currency { get; set; }

    #endregion

    #region Events

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize () {
        Instance = this;

        //TODO: temp until added to json
        Currency = DataConvert.SetCurrencyDefaults (2);
        Currency[0].Currency = "gold";
        currencyAmount = Currency[0].Amount = 0;
        Currency[0].Currency = "diamond";
        premiumCurrencyAmount = Currency[0].Amount = 50;

        goldText.text = currencyAmount.ToString ();
        diamondText.text = premiumCurrencyAmount.ToString ();
    }

    public void IncreaseCurrency (string currency, int amount) {
        switch (currency) {
            case "gold":
                currencyAmount += amount;
                break;
            case "diamonds":
                currencyAmount += amount;
                break;
        }
    }
    
    public void DecreaseCurrency (string currency, int amount) {
        switch (currency) {
            case "gold":
                currencyAmount -= amount;
                break;
            case "diamonds":
                currencyAmount -= amount;
                break;
        }
    }

    #endregion
}