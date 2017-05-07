using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
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
            //Fix attempt #1: No luck, didn't expect it to work anyway
            //light.GetComponent<CapsuleCollider>().enabled = !light.GetComponent<CapsuleCollider>().enabled;
        }
    }
}
