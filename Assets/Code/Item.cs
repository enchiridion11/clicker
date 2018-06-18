using System.Collections.Generic;

[System.Serializable]
public class Item {
    #region Fields

    public string name;
    //public string description;

    public List<ItemRequirements> requirements;

    #endregion

    #region Properties

    #endregion

    #region Constructors

    public Item (string name, List<ItemRequirements> requirements) {
        this.name = name;
        this.requirements = requirements;
    }

    #endregion

    #region Methods

    #region Unity

    #endregion

    public Dictionary<string, int> GetItemRequirements (string item) {
        var req = new Dictionary<string, int> ();
        for (var i = 0; i < requirements.Count; i++) {
            if (item != requirements[i].item) {
                req.Add (requirements[i].item, requirements[i].amount);
            }
        }

        return req;
    }

    #endregion
}

[System.Serializable]
public class ItemRequirements {
    public string item;
    public int amount;

    public ItemRequirements (string item, int amount) {
        this.item = item;
        this.amount = amount;
    }
}