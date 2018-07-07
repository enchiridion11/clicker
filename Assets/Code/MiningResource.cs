using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningResource : MonoBehaviour {
    #region Fields

    [SerializeField]
    string itemId;

    [SerializeField]
    UIBar bar;

    int currentClicks = 1;
    int requiredClicks;

    [SerializeField]
    ParticleSystem particles;

    [SerializeField]
    Texture particleImage;

    Material particleMat;

    #endregion

    #region Properties

    #endregion

    #region Events

    public Action<string> OnResourceMined;

    #endregion

    #region Methods

    #region Unity

    void OnDisable () {
        UnsubscribeFromEvents ();
    }

    #endregion

    public void Initialize () {
        SubscribeToEvents ();

        particleMat = GetComponent<ParticleSystemRenderer> ().material;
        particleMat.mainTexture = particleImage;

        requiredClicks = Data.GetItemData (itemId).Clicks;
        bar.Initialize (currentClicks, requiredClicks);
    }

    void SubscribeToEvents () {
    }

    void UnsubscribeFromEvents () {
    }

    public void Mine () {
        if (currentClicks < requiredClicks) {
            bar.gameObject.SetActive (true);
            bar.OnUpdateValue (currentClicks);
            particles.Play ();
            currentClicks++;
        }
        else {
            currentClicks = 1;
            bar.gameObject.SetActive (false);
            bar.SliderValue = 0;

            if (OnResourceMined != null) {
                OnResourceMined (itemId);
            }
        }
    }

    #endregion
}