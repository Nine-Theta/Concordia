using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldLight : MonoBehaviour {

    private Light _thisLight;
    private CapsuleCollider _thisCollider;
    private bool _beingHeld = false;
	// Use this for initialization
	void Start () {
        _thisLight = gameObject.GetComponent<Light>();
        _thisCollider = gameObject.GetComponent<CapsuleCollider>();
	}
	
	void FixedUpdate () {
        CheckHeld();
	}

    private void CheckHeld()
    {
        if (_beingHeld == true)
        {
            TurnOnLight();
            _beingHeld = false;
        }
        else
            TurnOffLight();
    }

    public void Hold()
    {
        _beingHeld = true;
    }

    public void TurnOffLight()
    {
        _thisLight.enabled = false;
        _thisCollider.enabled = false;
    }

    public void TurnOnLight()
    {
        _thisLight.enabled = true;
        _thisCollider.enabled = true;
    }
}
