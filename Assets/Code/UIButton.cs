using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    #region Fields

    Animator animator;

    bool isPressed;

    public PointerEventData CurrentEventData { get; private set; }

    #endregion

    #region Properties

    #endregion

    #region Methods

    #region Unity

    void OnEnable () {
        animator = GetComponent<Animator> ();
    }

    public void OnPointerDown (PointerEventData eventData) {
        CurrentEventData = eventData;
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
        if (isPressed && animator != null) {
            if (animator != null) {
                animator.Play ("ui_button_up");
            }

            isPressed = false;
        }
    }

    #endregion

    #endregion
}