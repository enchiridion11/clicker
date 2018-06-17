using System.Collections;
using UnityEngine;
using System.IO;

/// <summary>
/// Loads the JSON file and stores the data locally into the <c>Data</c> class.
/// </summary>
public class DataManager : MonoBehaviour {
    #region Fields

    [SerializeField]
    string dataFile = "data.json";

    #endregion

    #region Events

    #endregion

    #region Methods

    #region Unity

    void Awake () {
        StartCoroutine (LoadStatsFile ());
    }

    #endregion

    IEnumerator LoadStatsFile () {
        var jsonFilePath = Path.Combine (Application.streamingAssetsPath, dataFile);

        // if we're on the web, we have to wait until the WWW returns the real file path
        if (jsonFilePath.Contains ("://") || jsonFilePath.Contains (":///")) {
            var www = new WWW (jsonFilePath);
            yield return www;
            LoadStats (new JSONObject (www.text));
        }
        else {
            LoadStats (new JSONObject (File.ReadAllText (jsonFilePath)));
        }
    }

    void LoadStats (JSONObject data) {
        if (data != null) {
            Data.Items = DataConvert.JSONToItems (data["database"]);

            InventoryManager.Instance.Initialize ();
        }
        else {
            Debug.LogError ("Cannot load data!");
        }
    }

    #endregion
}