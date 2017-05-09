﻿using System.Collections;
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
        foreach (GameObject light in lights)
        {
            light.SetActive(!light.activeSelf);
        }
    }
}

[System.Serializable]
public enum TypeOfSwitch
{
    simpleToggle = 0,
    unpausePattern,
    holdToggle,
    flickeringToggle,
}