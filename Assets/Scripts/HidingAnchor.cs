using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingAnchor : MonoBehaviour
{
    //TODO: Automate this for reusability. My thoughts would be using the object's position to calculate these two positions
    //I considered removing them entirely because it seemed derpy but I get why you made this, still it shouldn't be set in the inspector
    //It should be made for reusability, so we can make a prefab and have it work the second you place it anywhere

    public Vector3 LightOffset;
    public Vector3 DarkOffset;

    public Vector3 LightLocation {
        get { return LightOffset + this.gameObject.transform.position; }
    }
    public Vector3 DarkLocation{
        get { return DarkOffset + this.gameObject.transform.position; }
    }
}

