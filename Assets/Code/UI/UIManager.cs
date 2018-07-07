using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour {
    #region Fields

    [SerializeField]
    Canvas canvas;

    [Header ("Parents"), SerializeField]
    Transform windows;

    [SerializeField]
    Transform dialogs;

    [SerializeField]
    GameObject overlay;

    [Header ("Prefabs"), SerializeField]
    GameObject sellParticles;

    RectTransform rect;

    #endregion

    #region Properties

    public static UIManager Instance { get; private set; }

    public Canvas Canvas {
        get { return canvas; }
    }

    public Transform Windows {
        get { return windows; }
    }

    public Transform Dialogs {
        get { return dialogs; }
    }

    public GameObject Overlay {
        get { return overlay; }
    }

    #endregion

    #region Events

    #endregion

    #region Methods

    #region Unity

    #endregion

    public void Initialize () {
        Instance = this;
    }

    public T OpenDialog<T> (string dialog, Transform parent, bool showOverlay = true) {
        overlay.SetActive (showOverlay);
        var go = UIWindowManager.Instance.GetDialog (dialog);
        go.transform.SetParent (parent);
        go.transform.localScale = Vector3.one;
        go.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;
        return go.GetComponent<T> ();
    }

    public T OpenWindow<T> (string window, Transform parent) {
        var go = UIWindowManager.Instance.GetDialog (window);
        go.transform.SetParent (parent);
        go.transform.localScale = Vector3.one;
        go.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;
        return go.GetComponent<T> ();
    }

    public Sprite GetItemIcon (string itemId) {
        return Resources.Load<Sprite> (string.Format ("Sprites/Items/item_{0}_01", itemId));
    }

    public void SellItemAnimation (Vector3 startPos) {
        var particles = Instantiate (sellParticles);
        var target = WorldToCanvasPosition (CurrencyManager.Instance.CurrencyIcon.position);
        particles.transform.SetParent (dialogs);
        particles.transform.localScale = Vector3.one;
        rect = particles.GetComponent<RectTransform> ();
        rect.anchoredPosition3D = WorldToCanvasPosition (startPos);

        iTween.ValueTo (rect.gameObject, iTween.Hash (
            "from", rect.anchoredPosition,
            "to", target,
            "time", 0.8f,
            "onupdatetarget", gameObject,
            "onupdate", "OnUpdate",
            "oncompletetarget", gameObject,
            "oncomplete", "OnCompleteDelay"
        ));
    }

    Vector2 WorldToCanvasPosition (Vector3 position) {
        var canvasRect = canvas.GetComponent<RectTransform> ();
        Vector2 temp = Camera.main.WorldToViewportPoint (position);

        temp.x *= canvasRect.sizeDelta.x;
        temp.y *= canvasRect.sizeDelta.y;

        temp.x -= canvasRect.sizeDelta.x * canvasRect.pivot.x;
        temp.y -= canvasRect.sizeDelta.y * canvasRect.pivot.y;

        return temp;
    }

    #region iTween
    
    void OnUpdate (Vector2 position) {
        rect.anchoredPosition = position;
    }

    void OnCompleteDelay () {
        Invoke ("OnComplete", 0.3f);
    }

    void OnComplete () {
        Destroy (rect.gameObject);
    }

    #endregion
    
    #endregion
}