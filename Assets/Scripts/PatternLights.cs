using System.Collections;
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
    public bool toggleFirstWhenSwitched = true;
    public List<PatternLight> lights;

    private bool _paused = false;
    //It's the first time, be gentle. 
    private bool _firstTime = true;
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
        if(_firstTime && toggleFirstWhenSwitched)
        {
            lights[_currentLight].duration = 0.0f;
        }
        lights[_currentLight].duration -= Time.deltaTime;
        if (lights[_currentLight].duration <= 0)
        {
            //reset the light duration for the next time it is called
            lights[_currentLight].duration = lights[_currentLight].maxDuration;
            //turn off this light and turn on the next
            lights[_currentLight].Toggle();
            _currentLight++;

            if (_currentLight >= lights.Count)
                _currentLight = 0;

            if(returnState && !_firstTime)
            {
                if (_currentLight > 0)
                    lights[_currentLight - 1].Toggle();
                else
                    lights[lights.Count].Toggle();
            }

            _firstTime = false;
        }
    }

    public void TogglePause()
    {
        _paused = !_paused;
    }
    
    /// <summary>
    /// Sets duration of all lights so the pattern lasts "duration" amount of seconds
    /// </summary>
    /// <param name="duration">Float duration in seconds</param>
    public void SetDuration(float duration)
    {
        foreach(PatternLight light in lights)
        {
            light.duration = duration / lights.Count;
            light.maxDuration = light.duration;
        }
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
            if (light == null)
                Debug.Log("Its name" + light.name + " Parent: " +  light.GetComponentInParent<Transform>().name);
            light.enabled = !light.enabled;
            if(light.GetComponent<CapsuleCollider>() != null)
                light.GetComponent<CapsuleCollider>().enabled = !light.GetComponent<CapsuleCollider>().enabled;
        }
    }
}
