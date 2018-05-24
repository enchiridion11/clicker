/// <summary>
/// Class containing all class structures that use the JSON file.
/// </summary>
[System.Serializable]
public class ResourceDataObject {
    public string name;
    public ResourceTypeDataObject type;
}

[System.Serializable]
public enum ResourceTypeDataObject {
    Stone,
    Copper,
    Iron,
    Coal,
    Wood,
    Gold,
    Diamonds
}

[System.Serializable]
public class ItemDataObject {
    public string name;
    public ItemTypeDataObject type;
    public ItemRequirementsDataObject[] requirements;
}

[System.Serializable]
public class ItemRequirementsDataObject {
    public string item;
    public int amount;
}

[System.Serializable]
public enum ItemTypeDataObject {
    Resource,
    Building,
    Mining,
    Automation,
    Miner,
    Recipe
}