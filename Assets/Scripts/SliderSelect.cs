using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSelect : ButtonSelect
{
    public Image targetGraphic;

    public RectTransform fillRect;
    public RectTransform handleRect;

    [Range(0.0f, 1.0f)]
    public float value;

    private void Update()
    {
        SliderManipulation();
        HandleColour();
    }

    private void SliderManipulation()
    {
        handleRect.localPosition = new Vector2((value - 0.515f) * 128, 0);
        fillRect.anchorMin = new Vector2(0, 0);
        fillRect.anchorMax = new Vector2(value, 1);
    }

    private void HandleColour()
    {
        switch (State)
        {
            case ButtonStates.Disabled:
                targetGraphic.color = buttonColour.disabledColor;
                break;
            case ButtonStates.Normal:
                targetGraphic.color = buttonColour.normalColor;
                break;
            case ButtonStates.Selected:
                targetGraphic.color = buttonColour.highlightedColor;
                break;
            case ButtonStates.Pressed:
                targetGraphic.color = buttonColour.pressedColor;
                break;
            default:
                targetGraphic.color = Color.magenta;
                break;
        }
    }
}