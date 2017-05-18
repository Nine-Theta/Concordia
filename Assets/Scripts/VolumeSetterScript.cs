using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSetterScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PauseMenu.UpdateMusicVolume += UpdateVolume;
	}

    public void UpdateVolume(float newVolume)
    {
        GetComponent<AudioSource>().volume = newVolume;
    }
}
