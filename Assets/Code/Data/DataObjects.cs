/// <summary>
/// Class containing all class structures that use the JSON file.
/// </summary>

[System.Serializable]
public class HumanDataObject {
    public string id;
    public string name;
    public int age;
    public bool canFly;
    public float speed;
    public PetDataObject pet;
}

[System.Serializable]
public class PetDataObject {
    public string id;
    public string name;
    public string owner;
    public int age;
    public bool canFly;
    public float speed;
}