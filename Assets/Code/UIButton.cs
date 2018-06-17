using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    #region Fields

    [SerializeField]
    Animator animator;

    [SerializeField]
    string buttonDownAnim;

    [SerializeField]
    string buttonUpAnim;

    bool isPressed;

    #endregion

    #region Properties

    #endregion

    #region Methods

    #region Unity

    public void OnPointerDown (PointerEventData eventData) {
        isPressed = true;
        animator.Play (buttonDownAnim);
    }

    public void OnPointerUp (PointerEventData eventData) {
        animator.Play (buttonUpAnim);
        isPressed = false;
    }

    public void OnPointerExit (PointerEventData eventData) {
        if (isPressed) {
            animator.Play (buttonUpAnim);
            isPressed = false;
        }
    }

    #endregion

    #endregion
}