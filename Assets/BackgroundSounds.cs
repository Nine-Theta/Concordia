using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSounds : MonoBehaviour {

    public AudioClip backgroundMusic;
    
	private void Awake () {
        if (backgroundMusic != null)
            GetComponent<AudioSource>().PlayOneShot(backgroundMusic);

    }
	private void FixedUpdate () {
        print(GetComponent<AudioSource>().isPlaying);
        //backgroundMusic.
    }
}
