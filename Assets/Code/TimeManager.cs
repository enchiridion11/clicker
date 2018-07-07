using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {
    #region Fields

    [SerializeField]
    int gameFrameRate = 32;

    [SerializeField]
    int defaultInterval = 32;

    [SerializeField]
    int timerInterval = 32;

    bool isRunning;

    Coroutine timerCoroutine;

    #endregion

    #region Properties

    public static TimeManager Instance { get; private set; }

    #endregion

    #region Events

    public Action<int> OnTick;

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize () {
        Instance = this;
        StartTimer ();
    }

    IEnumerator<int> Timer () {
        while (true) {
            timerInterval--;
            if (timerInterval < 0) {
                if (OnTick != null) {
                    OnTick.Invoke (timerInterval);
                }

                timerInterval = defaultInterval;
            }

            yield return gameFrameRate / defaultInterval;
        }
    }

    void StartTimer () {
        isRunning = true;
        timerCoroutine = StartCoroutine (Timer ());
    }

    void StopTimer () {
        isRunning = false;
        StopCoroutine (timerCoroutine);
    }

    #endregion
}