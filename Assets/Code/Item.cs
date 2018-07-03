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

    public List<ItemRequirements> GetItemRequirements (string itemId) {
        var req = new List<ItemRequirements> ();
        for (var i = 0; i < requirements.Count; i++) {
            if (itemId != requirements[i].item) {
                req.Add (new ItemRequirements (requirements[i].item, requirements[i].amount));
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
    public bool canCraft;

    #endregion

    #region Properties

    #endregion

    public ItemRequirements (string item, int amount, bool canCraft = false) {
        this.item = item;
        this.amount = amount;
        this.canCraft = canCraft;
    }
}