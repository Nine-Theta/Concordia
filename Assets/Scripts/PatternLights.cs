﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Monobehaviour applied to the parent object of the lights it controls
/// </summary>
public class PatternLights : MonoBehaviour
{
    //Returns the light to the previous state if true, else leaves in the current state
    public bool returnState = false;
    public bool pauseWhenDone = false;
    public bool startPaused = false;
    public List<PatternLight> lights;

    private bool _paused = false;
    private int _currentLight = 0;
    // Use this for initialization
    void Start()
    {
        foreach (PatternLight light in lights)
        {
            light.maxDuration = light.duration;
        }
        _paused = startPaused;
    }

    void FixedUpdate()
    {
        FollowPattern();
    }

    void FollowPattern()
    {
        if (_paused)
            return;
        lights[_currentLight].duration -= Time.deltaTime;
        if (lights[_currentLight].duration <= 0)
        {
            //reset the light duration for the next time it is called
            lights[_currentLight].duration = lights[_currentLight].maxDuration;
            //turn off this light and turn on the next
            if (returnState)
                lights[_currentLight].Toggle();
            _currentLight++;
            if (_currentLight >= lights.Count)
            {
                _currentLight = 0;
                if (pauseWhenDone)
                    _paused = true;
            }
            lights[_currentLight].Toggle();
        }
        
    }

    public void TogglePause()
    {
        _paused = !_paused;
    }
}

/// <summary>
/// class for a single "listing" of the PatternLights script
/// </summary>
/// 
[System.Serializable]
public class PatternLight
{
    public float duration;
    private float _maxDuration;
    public GameObject lightObject;


    public PatternLight()
    { }

    public float maxDuration
    {
        set { _maxDuration = value; }
        get { return _maxDuration; }
    }

    /// <summary>
    /// Toggles the lightobject connected to this listing
    /// </summary>
    public void Toggle()
    {
        foreach (Light light in lightObject.GetComponentsInChildren<Light>())
        {
            light.enabled = !light.enabled;
            light.GetComponent<CapsuleCollider>().enabled = !light.GetComponent<CapsuleCollider>().enabled;
        }
    }
}
