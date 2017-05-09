using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public TypeOfSwitch switchType = TypeOfSwitch.simpleToggle;
    public List<GameObject> lights;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Toggle()
    {
        switch (switchType)
        {
            case TypeOfSwitch.simpleToggle:
                foreach (GameObject light in lights)
                {
                    light.SetActive(!light.activeSelf);
                }
                break;
            case TypeOfSwitch.togglePausePattern:
                foreach(GameObject light in lights)
                {
                    light.GetComponent<PatternLights>().TogglePause();
                }
                break;
            case TypeOfSwitch.holdToggle:

                break;
            case TypeOfSwitch.flickeringPause:
                foreach(GameObject light in lights)
                {
                    light.GetComponent<FlickeringLights>().TogglePause();
                }
                break;
            case TypeOfSwitch.flickeringToggle:
                foreach (GameObject light in lights)
                {
                    light.GetComponent<FlickeringLights>().TogglePause();
                }
                break;
        }
    }
}

[System.Serializable]
public enum TypeOfSwitch
{
    simpleToggle = 0,
    togglePausePattern,
    holdToggle,
    flickeringPause,
    flickeringToggle
}