using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSounds : MonoBehaviour {

    public AudioClip doorOpenSound;
    

    private void Start()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        PauseMenu.UpdateSFXVolume += UpdateVolume;
    }

    public void PlayDoorOpenSound()
    {
        if(doorOpenSound != null)
            GetComponent<AudioSource>().PlayOneShot(doorOpenSound);
    }

    public void UpdateVolume(float newVolume)
    {
        GetComponent<AudioSource>().volume = newVolume;
    }
}
