using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonStates { Normal, Selected, Pressed, Disabled }

public class ButtonSelect : MonoBehaviour {

    public ColorBlock buttonColour;
    
    public ButtonStates State;

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
