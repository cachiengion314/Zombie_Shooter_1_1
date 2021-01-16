using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ButtonType {
    Up, Down, Right, Left, Shoot, FlashLight, ChangeWeapon, Escape, Throw
}

public class VirtualBttCanvas : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public ButtonType buttonType;

    [HideInInspector] public static bool isUpPress;
    [HideInInspector] public static bool isDownPress;
    [HideInInspector] public static bool isRightPress;
    [HideInInspector] public static bool isLeftPress;

    [HideInInspector] public static bool isShootPress;
    [HideInInspector] public static bool isShootClick;
    [HideInInspector] public static bool isShootHasClicked;

    [HideInInspector] public static bool isFlashLightPress;
    [HideInInspector] public static bool isFlashLightClick;

    [HideInInspector] public static bool isChangeWeaponClick;

    [HideInInspector] public static bool isEscapeClick;
    [HideInInspector] public static bool isThrowClick;

    private Image image;
    private Color currentColor;
    private void Awake() {
        image = GetComponent<Image>();
        currentColor = image.color;
    }

    public void OnPointerDown(PointerEventData eventData) {

        if (buttonType == ButtonType.Up) {
            isUpPress = true;
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, .7f);
        }
        if (buttonType == ButtonType.Down) {
            isDownPress = true;
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, .7f);
        }
        if (buttonType == ButtonType.Right) {
            isRightPress = true;
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, .7f);
        }
        if (buttonType == ButtonType.Left) {
            isLeftPress = true;
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, .7f);
        }
        if (buttonType == ButtonType.Shoot) {
            isShootPress = true;

            isShootClick = true;

            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, .7f);
        }
        if (buttonType == ButtonType.FlashLight) {
            isFlashLightPress = true;
            if (!isFlashLightClick) {
                isFlashLightClick = true;
            }
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, .7f);
        }
        if (buttonType == ButtonType.ChangeWeapon) {
            isChangeWeaponClick = true;
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, .7f);
        }
        if (buttonType == ButtonType.Escape) {
            isEscapeClick = true;
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, .7f);
        }
        if (buttonType == ButtonType.Throw) {
            isThrowClick = true;
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, .7f);
        }
    }

    public void OnPointerUp(PointerEventData eventData) {

        if (buttonType == ButtonType.Up) {
            isUpPress = false;
            image.color = currentColor;
        }
        if (buttonType == ButtonType.Down) {
            isDownPress = false;
            image.color = currentColor;
        }
        if (buttonType == ButtonType.Right) {
            isRightPress = false;
            image.color = currentColor;
        }
        if (buttonType == ButtonType.Left) {
            isLeftPress = false;
            image.color = currentColor;
        }
        if (buttonType == ButtonType.Shoot) {
            isShootPress = false;
            isShootClick = false;
            image.color = currentColor;
        }
        if (buttonType == ButtonType.FlashLight) {
            isFlashLightPress = false;
            isFlashLightClick = false;
            image.color = currentColor;
        }
        if (buttonType == ButtonType.ChangeWeapon) {
            isChangeWeaponClick = false;
            image.color = currentColor;
        }
        if (buttonType == ButtonType.Escape) {
            isEscapeClick = false;
            image.color = currentColor;
        }
        if (buttonType == ButtonType.Throw) {
            isThrowClick = false;
            image.color = currentColor;
        }
    }
}
