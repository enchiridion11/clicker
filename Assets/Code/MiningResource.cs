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
    MiningBar bar;

    [SerializeField]
    int requiredTaps;

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
    }

    void SubscribeToEvents () {
        bar.OnMaxTaps += OnMaxTaps;
    }

    void UnsubscribeFromEvents () {
        bar.OnMaxTaps -= OnMaxTaps;
    }

    public void OnTap () {
        bar.gameObject.SetActive (true);
        bar.Initialize (requiredTaps);
        bar.Increase ();
        particles.Play ();
    }

    void OnMaxTaps () {
        if (OnResourceMined != null) {
            OnResourceMined (itemId);
        }
    }

    #endregion
}