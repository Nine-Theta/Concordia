using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public TypeOfSwitch switchType = TypeOfSwitch.simpleToggle;
    public List<GameObject> lights;
    public float duration = 0.0f;

    private float currentTimer = 0.0f;
    private bool turnOn = false;
    // Use this for initialization
    void Start()
    {

    }

    void FixedUpdate()
    {
        CheckTime();
    }

    private void CheckTime()
    {
        if (switchType == TypeOfSwitch.timedSwitch && currentTimer > 0.0f)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer >= 0.0f)
            {
                foreach (GameObject light in lights)
                {
                    light.GetComponent<Light>().enabled = !light.GetComponent<Light>().enabled;
                    light.GetComponent<CapsuleCollider>().enabled = !light.GetComponent<CapsuleCollider>().enabled;
                }
            }
        }
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
                foreach (GameObject light in lights)
                {
                    light.GetComponent<PatternLights>().TogglePause();
                }
                break;
            case TypeOfSwitch.holdToggle:
                foreach (GameObject light in lights)
                {
                    light.GetComponent<HoldLight>().Hold();
                }
                break;
            case TypeOfSwitch.flickeringPause:
                foreach (GameObject light in lights)
                {
                    light.GetComponent<FlickeringLights>().TogglePause();
                }
                break;
            case TypeOfSwitch.flickeringToggle:
                if (turnOn)
                {
                    foreach (GameObject parent in lights)
                    {
                        foreach (Light light in parent.GetComponentsInChildren<Light>())
                        {
                            light.GetComponent<FlickeringLights>().TurnOn();
                        }
                    }
                }
                else
                {
                    foreach (GameObject parent in lights)
                    {
                        foreach (Light light in parent.GetComponentsInChildren<Light>())
                        {
                            light.GetComponent<FlickeringLights>().TurnOff();
                        }
                    }
                }
                break;
            case TypeOfSwitch.searchInChildrenToggle:
                foreach(GameObject parent in lights)
                {
                    foreach(Light light in parent.GetComponentsInChildren<Light>())
                    {
                        light.enabled = !light.enabled;
                        light.GetComponent<CapsuleCollider>().enabled = !light.GetComponent<CapsuleCollider>().enabled;
                    }
                }
                break;
            case TypeOfSwitch.timedSwitch:
                if (currentTimer <= 0.0f)
                {
                    foreach (GameObject light in lights)
                    {
                        light.GetComponent<Light>().enabled = !light.GetComponent<Light>().enabled;
                        light.GetComponent<CapsuleCollider>().enabled = !light.GetComponent<CapsuleCollider>().enabled;
                        currentTimer = duration;
                    }
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
    flickeringToggle,
    searchInChildrenToggle,
    timedSwitch
}