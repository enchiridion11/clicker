using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    #region Fields

    [SerializeField]
    string id;

    [SerializeField]
    List<ItemRequirements> requirements;

    #endregion

    #region Properties

    public string Id {
        get { return id; }
        set { id = value; }
    }

    public List<ItemRequirements> Requirements {
        get { return requirements; }
        set { requirements = value; }
    }

    #endregion

    #region Constructors

    public Item (string id, List<ItemRequirements> requirements) {
        this.id = id;
        this.requirements = requirements;
    }

    #endregion

    #region Methods

    #region Unity

    #endregion

    public Dictionary<string, int> GetItemRequirements (string itemId) {
        var req = new Dictionary<string, int> ();
        for (var i = 0; i < requirements.Count; i++) {
            if (itemId != requirements[i].item) {
                req.Add (requirements[i].item, requirements[i].amount);
            }
        }

        return req;
    }

    #endregion
}

[System.Serializable]
public class ItemRequirements {
    #region Fields

    public string item;
    public int amount;

    #endregion


    #region Properties

    #endregion

    public ItemRequirements (string item, int amount) {
        this.item = item;
        this.amount = amount;
    }
}