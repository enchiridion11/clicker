using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Example class presenting the data from JSON in UI.
/// </summary>
public class HumanCardPresenter : MonoBehaviour {
    #region Fields

    [SerializeField]
    Text nameText;

    [SerializeField]
    Text ageText;

    [SerializeField]
    Text canFlyTExt;

    [SerializeField]
    Text speedText;
    
    [SerializeField]
    Text petText;

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Set (string name, int age, bool canFly, float speed, Pet pet) {
        nameText.text = name;
        ageText.text = age.ToString();
        canFlyTExt.text = canFly ? "Yes" : "no";
        speedText.text = speed.ToString();

        if (pet != null) {
            petText.text = pet.Name;
        }
    }

    #endregion
}