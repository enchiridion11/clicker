/// <summary>
/// Example class defining a pet.
/// </summary>
public class Pet {
    #region Properties

    public string ID { get; set; }
    public string Name { get; set; }
    public string Owner { get; set; }
    public int Age { get; set; }
    public bool CanFly { get; set; }
    public float Speed { get; set; }

    #endregion

    #region Constructors

    public Pet (string id, string name, string owner, int age, bool canFly, float speed) {
        ID = id;
        Name = name;
        Owner = owner;
        Age = age;
        CanFly = canFly;
        Speed = speed;
    }

    #endregion
}