using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    #region Fields

    [SerializeField]
    float holdTime;

    [Space (5)]
    public UnityEvent OnHold;

    [Space (5)]
    public UnityEvent OnClick;

    [Space (5)]
    public UnityEvent OnRelease;

    Animator animator;

    bool isPressed;
    bool holdEventFired;

    float elapsedTime;

    #endregion

    #region Properties

    #endregion

    #region Methods

    #region Unity

    void OnEnable () {
        animator = GetComponent<Animator> ();
    }

    void Update () {
        if (isPressed && !holdEventFired) {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= holdTime) {
                if (OnHold != null) {
                    Reset ();
                    holdEventFired = true;
                    OnHold.Invoke ();
                }
            }
        }
    }

    public void OnPointerDown (PointerEventData eventData) {
        if (animator != null) {
            animator.Play ("ui_button_down");
        }

        isPressed = true;
    }

    public void OnPointerUp (PointerEventData eventData) {
        if (animator != null) {
            animator.Play ("ui_button_up");
        }

        if (holdEventFired) {
            if (OnRelease != null) {
                holdEventFired = false;
                OnRelease.Invoke ();
                Reset ();
            }

            return;
        }

        if (OnClick != null) {
            holdEventFired = false;
            OnClick.Invoke ();
            Reset ();
        }
    }

    public void OnPointerExit (PointerEventData eventData) {
        if (isPressed && animator != null) {
            if (animator != null) {
                animator.Play ("ui_button_up");
            }

            if (OnRelease != null) {
                holdEventFired = false;
                OnRelease.Invoke ();
                Reset ();
            }
        }
    }

    void Reset () {
        isPressed = false;
        elapsedTime = 0;
    }

    #endregion

    #endregion
}