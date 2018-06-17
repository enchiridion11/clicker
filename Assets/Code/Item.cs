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

    public Item (string name,  List<ItemRequirements> requirements) {
        this.name = name;
        this.requirements = requirements;
    }

    #endregion

    #region Methods

    #region Unity

    #endregion

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