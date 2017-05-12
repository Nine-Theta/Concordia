using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LightSwitch : MonoBehaviour
{
    public TypeOfSwitch switchType = TypeOfSwitch.simpleToggle;
    public List<GameObject> lights;
    public float duration = 0.0f;
    public float cooldownTimer = 0.0f;
    public AudioClip toggleOnSound;
    public AudioClip toggleOffSound;
    public AudioClip brokenSound;

    private float _currentTimer = 0.0f;
    private float _currentCooldownTimer = 0.0f;
    private bool _turnOn = false;
    private bool _usable = true;
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
        if (switchType == TypeOfSwitch.timedSwitch && _currentTimer > 0.0f)
        {
            _currentTimer -= Time.deltaTime;
            if (_currentTimer <= 0.0f)
            {
                foreach (GameObject parent in lights)
                {
                    foreach (Light light in parent.GetComponentsInChildren<Light>())
                    {
                        light.GetComponent<Light>().enabled = !light.GetComponent<Light>().enabled;
                        light.GetComponent<CapsuleCollider>().enabled = !light.GetComponent<CapsuleCollider>().enabled;
                        _currentCooldownTimer = cooldownTimer;
                    }
                }
            }
        }
        if(_currentCooldownTimer > 0.0f)
        {
            _currentCooldownTimer -= Time.deltaTime;
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
                foreach (GameObject parent in lights)
                {
                    foreach (Light light in parent.GetComponentsInChildren<Light>())
                    {
                        light.GetComponent<HoldLight>().Hold();
                    }
                }
                break;
            case TypeOfSwitch.flickeringPause:
                foreach (GameObject parent in lights)
                {
                    foreach (Light light in parent.GetComponentsInChildren<Light>())
                    {
                        light.GetComponent<FlickeringLights>().TogglePause();
                    }
                }
                break;
            case TypeOfSwitch.flickeringToggle:
                if (_turnOn)
                {
                    if (toggleOnSound != null)
                        GetComponent<AudioSource>().PlayOneShot(toggleOnSound);
                    foreach (GameObject parent in lights)
                    {
                        foreach (Light light in parent.GetComponentsInChildren<Light>())
                        {
                            if (light.GetComponent<FlickeringLights>() != null)
                                light.GetComponent<FlickeringLights>().TurnOn();
                            else
                                light.gameObject.AddComponent<FlickeringLights>().TurnOn();
                        }
                    }
                }
                else
                {
                    if (toggleOffSound != null)
                        GetComponent<AudioSource>().PlayOneShot(toggleOffSound);
                    foreach (GameObject parent in lights)
                    {
                        foreach (Light light in parent.GetComponentsInChildren<Light>())
                        {
                            if (light.GetComponent<FlickeringLights>() != null)
                                light.GetComponent<FlickeringLights>().TurnOff();
                            else
                                light.gameObject.AddComponent<FlickeringLights>().TurnOn();
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
                if (_currentTimer <= 0.0f && _currentCooldownTimer <= 0.0f)
                {
                    foreach (GameObject parent in lights)
                    {
                        foreach (Light light in parent.GetComponentsInChildren<Light>())
                        {
                            light.GetComponent<Light>().enabled = !light.GetComponent<Light>().enabled;
                            light.GetComponent<CapsuleCollider>().enabled = !light.GetComponent<CapsuleCollider>().enabled;
                            _currentTimer = duration;
                        }
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