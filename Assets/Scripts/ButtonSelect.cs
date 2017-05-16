using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour {

    public ColorBlock buttonColour;

    public enum ButtonStates { Normal, Selected, Pressed, Disabled }

    public ButtonStates State;

    private bool _selected = false;

    public bool Selected
    {
        set { _selected = value; }
        get { return _selected; }
    }

    private void CheckState()
    {
        switch (State)
        {
            case ButtonStates.Disabled:
                gameObject.GetComponent<Image>().color = buttonColour.disabledColor;
                break;
            case ButtonStates.Normal:
                gameObject.GetComponent<Image>().color = buttonColour.normalColor;
                break;
            case ButtonStates.Selected:
                gameObject.GetComponent<Image>().color = buttonColour.highlightedColor;
                break;
            case ButtonStates.Pressed:
                gameObject.GetComponent<Image>().color = buttonColour.pressedColor;
                break;
            default:
                gameObject.GetComponent<Image>().color = Color.magenta;
                break;
        }
    }

    private void Update()
    {
        CheckState();
    }
}
