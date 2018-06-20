using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    #region Fields

    Animator animator;

    bool isPressed;

    #endregion

    #region Properties

    #endregion

    #region Methods

    void OnEnable () {
        animator = GetComponent<Animator> ();
    }

    #region Unity

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

        isPressed = false;
    }

    public void OnPointerExit (PointerEventData eventData) {
        if (isPressed && animator) {
            if (animator != null) {
                animator.Play ("ui_button_up");
            }

            isPressed = false;
        }
    }

    #endregion

    #endregion
}