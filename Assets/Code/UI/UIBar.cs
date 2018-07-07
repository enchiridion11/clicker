using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour {
    #region Fields

    [SerializeField]
    float animationSpeed = 1f;

    [SerializeField]
    Slider slider;

    [SerializeField]
    TextMeshProUGUI currentAmountText;

    [SerializeField]
    TextMeshProUGUI maxAmountText;

    #endregion

    #region Properties

    public float SliderValue {
        get { return slider.value; }
        set { slider.value = value; }
    }

    #endregion

    #region Events

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize (int current, int max) {
        slider.value = current;
        slider.maxValue = max;

        if (currentAmountText != null && maxAmountText != null) {
            currentAmountText.text = current.ToString ();
            maxAmountText.text = max.ToString ();
        }
    }

    public void OnUpdateValue (int newValue) {
        if (animationSpeed > 0) {
            print ("slider.value: " + slider.value);
            iTween.ValueTo (gameObject, iTween.Hash (
                "from", slider.value,
                "to", (float) newValue,
                "time", animationSpeed,
                "onupdatetarget", gameObject,
                "onupdate", "OnUpdate"
            ));
        }
        else {
            slider.value = newValue;
            print ("hello");
        }

        if (currentAmountText != null) {
            currentAmountText.text = newValue.ToString ();
        }
    }
    
    #region iTween

    void OnUpdate (float value) {
        slider.value = value;
    }

    #endregion

    #endregion
}