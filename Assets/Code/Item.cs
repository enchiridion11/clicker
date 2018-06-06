using UnityEngine;

[System.Serializable]
public class Item {
    #region Fields

    [SerializeField]
    public string name;

    [SerializeField]
    public ItemRequirements[] requirements;

    #endregion

    #region Properties

    #endregion

    #region Constructors

    public Item (string name, ItemRequirements[] requirements) {
        //ID = id;
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