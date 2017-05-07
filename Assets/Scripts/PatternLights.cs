using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternLights : MonoBehaviour
{
    public List<PatternLight> lights;

    private int currentLight = 0;
    // Use this for initialization
    void Start()
    {
        foreach (PatternLight light in lights)
        {
            light.maxDuration = light.duration;
        }
    }

    void FixedUpdate()
    {
        FollowPattern();
    }

    void FollowPattern()
    {
        lights[currentLight].duration -= Time.deltaTime;
        if (lights[currentLight].duration <= 0)
        {
            //reset the light duration for the next time it is called
            lights[currentLight].duration = lights[currentLight].maxDuration;
            //turn off this light and turn on the next
            lights[currentLight].Toggle();
            currentLight++;
            if (currentLight >= lights.Count)
                currentLight = 0;
            lights[currentLight].Toggle();
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
        lightObject.SetActive(!lightObject.activeSelf);
    }
}
