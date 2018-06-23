using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that serializes data to and from the JSON file.
/// </summary>
public static class DataConvert {
    #region Methods

    public static Item[] JSONToItems (JSONObject json) {
        var items = new List<Item> ();
        for (var i = 0; i < json.list.Count; i++) {
            var data = json.list[i];
            items.Add (new Item (
                data.ToStringSafe ("id"),
                data.ToIntSafe ("clicks"),
                data.ToIntSafe ("sellAmount"),
                data.HasField ("requirements") ? JSONToItemRequirements (data["requirements"]) : null
            ));
        }

        return items.ToArray ();
    }

    public static List<Item> JSONToItemDatabase (JSONObject json) {
        var items = new List<Item> ();
        for (var i = 0; i < json.list.Count; i++) {
            var data = json.list[i];
            items.Add (new Item (
                data.ToStringSafe ("id"),
                data.ToIntSafe ("clicks"),
                data.ToIntSafe ("sellAmount"),
                data.HasField ("requirements") ? JSONToItemRequirements (data["requirements"]) : null
            ));
        }

        return items;
    }

    public static ItemRequirements JSONToItemRequirement (JSONObject json) {
        return new ItemRequirements (
            json.ToStringSafe ("id"),
            json.ToIntSafe ("amount"));
    }

    public static List<ItemRequirements> JSONToItemRequirements (JSONObject json) {
        var requirements = new List<ItemRequirements> ();
        for (var i = 0; i < json.list.Count; i++) {
            var data = json.list[i];
            requirements.Add (new ItemRequirements (
                data.ToStringSafe ("item"),
                data.ToIntSafe ("amount")
            ));
        }

        return requirements;
    }

    public static CurrencyDefaults[] JSONToCurrencyDefaults (JSONObject json) {
        var currencies = new List<CurrencyDefaults> ();
        for (var i = 0; i < json.list.Count; i++) {
            var data = json.list[i];
            currencies.Add (new CurrencyDefaults (
                data.ToStringSafe ("currency"),
                data.ToIntSafe ("amount")
            ));
        }

        return currencies.ToArray ();
    }

    //TODO: temp until added to json
    public static CurrencyDefaults[] SetCurrencyDefaults (int count) {
        var currencies = new List<CurrencyDefaults> ();
        for (var i = 0; i < count; i++) {
            currencies.Add (new CurrencyDefaults ("", 0));
        }

        return currencies.ToArray ();
    }

    static string ToStringSafe (this JSONObject json, string property) {
        return json.HasField (property) ? json[property].str : string.Empty;
    }

    static int ToIntSafe (this JSONObject json, string property, int value = 0) {
        return json.HasField (property) ? (int) json[property].n : value;
    }

    static double ToDoubleSafe (this JSONObject json, string property, double value = 0) {
        return json.HasField (property) ? json[property].n : value;
    }

    static float ToFloatSafe (this JSONObject json, string property) {
        return json.HasField (property) ? json[property].n : 0.0f;
    }

    static bool ToBoolSafe (this JSONObject json, string property, bool value = false) {
        return json.HasField (property) ? json[property].b : value;
    }

    #endregion
}