using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LightSwitch : MonoBehaviour
{

    public Light lightswitchLight;
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
    private bool _patternAssigned = false;
    // Use this for initialization
    private void Start()
    {
        if (gameObject.GetComponent<AudioSource>() != null)
        {
            gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
            PauseMenu.UpdateSFXVolume += UpdateVolume;
        }
    }

    private void FixedUpdate()
    {
        CheckTime();
    }

    public void UpdateVolume(float newVolume)
    {
        GetComponent<AudioSource>().volume = newVolume;
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
            if (_currentCooldownTimer <= 0.0f)
                _usable = true;
        }
    }

    public void Toggle()
    {
        if (!_usable)
            return;
        switch (switchType)
        {
            case TypeOfSwitch.simpleToggle:
                if (toggleOnSound != null)
                {
                    if (lightswitchLight.enabled)
                        GetComponent<AudioSource>().PlayOneShot(toggleOnSound);
                    else
                        GetComponent<AudioSource>().PlayOneShot(toggleOffSound);
                }

                foreach (GameObject light in lights)
                {
                    light.SetActive(!light.activeSelf);
                }
                lightswitchLight.enabled = !lightswitchLight.enabled;
                break;

            case TypeOfSwitch.togglePausePattern:
                if (toggleOnSound != null)
                {
                    if (lightswitchLight.enabled)
                        GetComponent<AudioSource>().PlayOneShot(toggleOnSound);
                    else
                        GetComponent<AudioSource>().PlayOneShot(toggleOffSound);
                }
                if(_patternAssigned == false && cooldownTimer > 0.0f)
                {
                    foreach(GameObject light in lights)
                    {
                        light.GetComponent<PatternLights>().SetDuration(duration);
                    }
                    _patternAssigned = true;
                }
                foreach (GameObject light in lights)
                {
                    light.GetComponent<PatternLights>().TogglePause();
                }
                lightswitchLight.enabled = !lightswitchLight.enabled;
                if(cooldownTimer > 0.0f)
                {
                    _currentCooldownTimer = cooldownTimer;
                    _usable = false;
                }
                break;

            case TypeOfSwitch.holdToggle:
                foreach (GameObject parent in lights)
                {
                    foreach (Light light in parent.GetComponentsInChildren<Light>())
                    {
                        if (light.GetComponent<HoldLight>() != null)
                            light.GetComponent<HoldLight>().Hold();
                        else
                            light.gameObject.AddComponent<HoldLight>().Hold();
                    }
                }
                break;

            case TypeOfSwitch.flickeringPause:
                if (toggleOnSound != null)
                {
                    if (lightswitchLight.enabled)
                        GetComponent<AudioSource>().PlayOneShot(toggleOnSound);
                    else
                        GetComponent<AudioSource>().PlayOneShot(toggleOffSound);
                }
                foreach (GameObject parent in lights)
                {
                    foreach (Light light in parent.GetComponentsInChildren<Light>())
                    {
                        light.GetComponent<FlickeringLights>().TogglePause();
                    }
                }
                lightswitchLight.enabled = !lightswitchLight.enabled;
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
                    lightswitchLight.enabled = true;
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
                    lightswitchLight.enabled = false;
                }
                break;

            case TypeOfSwitch.searchInChildrenToggle:
                if (toggleOnSound != null)
                {
                    if (lightswitchLight.enabled)
                        GetComponent<AudioSource>().PlayOneShot(toggleOnSound);
                    else
                        GetComponent<AudioSource>().PlayOneShot(toggleOffSound);
                }
                foreach (GameObject parent in lights)
                {
                    foreach(Light light in parent.GetComponentsInChildren<Light>())
                    {
                        light.enabled = !light.enabled;
                        if (light.GetComponent<CapsuleCollider>() != null)
                            light.GetComponent<CapsuleCollider>().enabled = !light.GetComponent<CapsuleCollider>().enabled;
                    }
                }
                lightswitchLight.enabled = !lightswitchLight.enabled;
                break;

            case TypeOfSwitch.timedSwitch:
                if (_currentTimer <= 0.0f && _currentCooldownTimer <= 0.0f)
                {
                    if (toggleOnSound != null)
                    {
                        if (lightswitchLight.enabled)
                            GetComponent<AudioSource>().PlayOneShot(toggleOnSound);
                        else
                            GetComponent<AudioSource>().PlayOneShot(toggleOffSound);
                    }
                    foreach (GameObject parent in lights)
                    {
                        foreach (Light light in parent.GetComponentsInChildren<Light>())
                        {
                            light.GetComponent<Light>().enabled = !light.GetComponent<Light>().enabled;
                            if (light.GetComponent<CapsuleCollider>() != null)
                                light.GetComponent<CapsuleCollider>().enabled = !light.GetComponent<CapsuleCollider>().enabled;
                            _currentTimer = duration;
                        }
                    }
                    _usable = false;
                    lightswitchLight.enabled = !lightswitchLight.enabled;
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