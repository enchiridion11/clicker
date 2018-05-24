/// <summary>
/// Class containing all JSON data locally.
/// </summary>
public static class Data {
    #region Fields

    public static Human[] Humans;
    public static Pet[] Pets;

    #endregion

    #region Methods

    public static Human GetHumanData (string id) {
        Human data = null;
        foreach (var human in Humans) {
            if (human.Name == id) {
                data = human;
            }
        }
        return data;
    }
    
    public static Pet GetPetData (string id) {
        Pet data = null;
        foreach (var pet in Pets) {
            if (pet.Name == id) {
                data = pet;
            }
        }
        return data;
    }

    #endregion
}