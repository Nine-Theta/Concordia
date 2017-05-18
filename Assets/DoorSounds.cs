using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSounds : MonoBehaviour {

    public AudioClip doorOpenSound;
    

    private void Start()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
    }

    public void PlayDoorOpenSound()
    {
        if(doorOpenSound != null)
            GetComponent<AudioSource>().PlayOneShot(doorOpenSound);
    }
}
