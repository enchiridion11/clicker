/// <summary>
/// Example class defining a human.
/// </summary>
public class Human {
    #region Properties

    public string ID { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public bool CanFly { get; set; }
    public float Speed { get; set; }
    public Pet Pet { get; set; }

    #endregion

    #region Constructors

    public Human (string id, string name, int age, bool canFly, float speed, Pet pet) {
        ID = id;
        Name = name;
        Age = age;
        CanFly = canFly;
        Speed = speed;
        Pet = pet;
    }

    #endregion
}