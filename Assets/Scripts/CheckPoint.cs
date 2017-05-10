using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {
    public int index = 0;
    //Boolean decides if this checkpoint can be the active checkpoint regardless of index
    //So if the player can "return" to this checkpoint and have it become the active one
    public bool returnAble = false;

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
