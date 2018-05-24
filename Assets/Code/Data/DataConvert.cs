using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that serializes data to and from the JSON file.
/// </summary>
public static class DataConvert {
    #region Methods

    public static Human[] JSONToHumans (JSONObject json) {
        var humans = new List<Human>();
        for (var i = 0; i < json.list.Count; i++) {
            var data = json.list[i];
            humans.Add(new Human(
                data.ToStringSafe("id"),
                data.ToStringSafe("name"),
                data.ToIntSafe("age"),
                data.ToBoolSafe("canFly"),
                data.ToFloatSafe("speed"),
                data.HasField("pet") ? JSONToPet(data["pet"]) : null
            ));
        }

        return humans.ToArray();
    }

    public static Pet JSONToPet (JSONObject json) {
        return new Pet(
            json.ToStringSafe("id"),
            json.ToStringSafe("name"),
            json.ToStringSafe("owner"),
            json.ToIntSafe("age"),
            json.ToBoolSafe("canFly"),
            json.ToFloatSafe("speed"));
    }
    
    public static Pet[] JSONToPets (JSONObject json) {
        var pet = new List<Pet>();
        for (var i = 0; i < json.list.Count; i++) {
            var data = json.list[i];
            pet.Add(new Pet(
                data.ToStringSafe("id"),
                data.ToStringSafe("name"),
                data.ToStringSafe("owner"),
                data.ToIntSafe("age"),
                data.ToBoolSafe("canFly"),
                data.ToFloatSafe("speed")
            ));
        }

        return pet.ToArray();
    }

    static string ToStringSafe (this JSONObject json, string property) {
        return json.HasField(property) ? json[property].str : string.Empty;
    }

    static int ToIntSafe (this JSONObject json, string property, int value = 0) {
        return json.HasField(property) ? (int) json[property].n : value;
    }

    static double ToDoubleSafe (this JSONObject json, string property, double value = 0) {
        return json.HasField(property) ? json[property].n : value;
    }

    static float ToFloatSafe (this JSONObject json, string property) {
        return json.HasField(property) ? json[property].n : 0.0f;
    }

    static bool ToBoolSafe (this JSONObject json, string property, bool value = false) {
        return json.HasField(property) ? json[property].b : value;
    }

    #endregion
}