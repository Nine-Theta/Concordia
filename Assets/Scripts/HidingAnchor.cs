using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingAnchor : MonoBehaviour
{
    public Vector3 LightOffset;
    public Vector3 DarkOffset;

    public Vector3 LightLocation
    {
        get { return LightOffset + this.gameObject.transform.position; }
    }
    public Vector3 DarkLocation
    {
        get { return DarkOffset + this.gameObject.transform.position; }
    }
}

