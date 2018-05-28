using UnityEngine;

[System.Serializable]
public class Item {
    #region Fields

    [SerializeField]
    string name;

    [SerializeField]
    ItemRequirements itemRequirements;

    #endregion

    #region Properties

    public string Name {
        get { return name; }

        set { name = value; }
    }

    public ItemRequirements ItemRequirements {
        get { return itemRequirements; }

        set { itemRequirements = value; }
    }

    #endregion

    #region Constructors

    public Item (string name) {
        //ID = id;
        Name = name;
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
}