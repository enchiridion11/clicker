using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    #region Fields

    [SerializeField]
    string id;

    [SerializeField]
    int clicks;
    
    [SerializeField]
    int sellAmount;
    
    [SerializeField]
    List<ItemRequirements> requirements;

    #endregion

    #region Properties

    public string Id {
        get { return id; }
        set { id = value; }
    }

    public int Clicks {
        get { return clicks; }
        set { clicks = value; }
    }
    
    public int SellAmount {
        get { return sellAmount; }
        set { sellAmount = value; }
    }

    public List<ItemRequirements> Requirements {
        get { return requirements; }
        set { requirements = value; }
    }

    #endregion

    #region Constructors

    public Item (string id, int clicks, int sellAmount, List<ItemRequirements> requirements) {
        this.id = id;
        this.clicks = clicks;
        this.sellAmount = sellAmount;
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