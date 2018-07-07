using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    #region Fields

    [SerializeField]
    bool animate = true;

    [Space (5)]
    public UnityEvent OnClick;

    Animator animator;

    bool isPressed;
    bool isOverButton;

    #endregion

    #region Properties

    #endregion

    #region Methods

    #region Unity

    void OnEnable () {
        animator = GetComponent<Animator> ();
    }

    public void OnPointerDown (PointerEventData eventData) {
        if (animator != null && animate) {
            animator.Play ("ui_button_down");
        }

        isPressed = true;
        isOverButton = true;
    }

    public void OnPointerUp (PointerEventData eventData) {
        if (animator != null && animate) {
            animator.Play ("ui_button_up");
        }

        if (isOverButton) {
            if (OnClick != null) {
                OnClick.Invoke ();
            }
        }

        isPressed = false;
    }

    public void OnPointerExit (PointerEventData eventData) {
        if (isPressed && animator != null && animate) {
            animator.Play ("ui_button_up");

            isPressed = false;
            isOverButton = false;
        }
    }

    #endregion

    #endregion
}