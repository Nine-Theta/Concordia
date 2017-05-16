using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSounds : MonoBehaviour {

    public AudioClip doorOpenSound;
    
    public void PlayDoorOpenSound()
    {
        if(doorOpenSound != null)
            GetComponent<AudioSource>().PlayOneShot(doorOpenSound);
    }
}
