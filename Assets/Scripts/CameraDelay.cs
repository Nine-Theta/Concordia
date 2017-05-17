using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDelay : MonoBehaviour {
    public float delayInSeconds = 1.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        delayInSeconds -= Time.deltaTime;
		if(delayInSeconds <= 0.0f)
        {
            gameObject.GetComponent<Camera>().enabled = false;
            this.enabled = false;
        }
	}
}
