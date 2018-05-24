using UnityEngine;

// example class which executes once data has been loaded from json
public class DataVisualiser : MonoBehaviour {
    #region Fields

    [SerializeField]
    GameObject humanCardPrefab;

    [SerializeField]
    Transform canvasParent;

    public static DataVisualiser Instance { get; private set; }

    #endregion

    #region Properties

    #endregion

    #region Methods

    #region Unity

    void Awake () {
        Instance = this;
    }

    #endregion

    public void Initialize () {
        CreateExampleObjects();
    }

    void CreateExampleObjects () {
        var offset = -600;
        foreach (var human in Data.Humans) {
            var card = Instantiate(humanCardPrefab).GetComponent<HumanCardPresenter>();
            card.Set(human.Name, human.Age, human.CanFly, human.Speed, human.Pet);
            card.transform.SetParent(canvasParent);
            card.transform.name = "Example " + card.name;
            card.transform.localScale = Vector3.one;
            card.transform.localPosition = new Vector3(offset, 0, 0);
            offset += 600;
        }
    }

    #endregion
}