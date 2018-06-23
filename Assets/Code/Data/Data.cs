using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class containing all JSON data locally.
/// </summary>
public static class Data {
    #region Fields

    public static Item[] Items;

    public static CurrencyDefaults[] CurrencyDefaults;

    #endregion

    #region Methods

    public static Item GetItemData (string id) {
        Item data = null;
        foreach (var item in Items) {
            if (item.Id == id) {
                data = item;
            }
        }

        return data;
    }

    #endregion
}

public class CurrencyDefaults {
    public string Currency { get; set; }

    public int Amount { get; set; }

    public CurrencyDefaults (string currency, int amount) {
        Currency = currency;
        Amount = amount;
    }
}