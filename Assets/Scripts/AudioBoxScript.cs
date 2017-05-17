﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBoxScript : MonoBehaviour {
    /// <summary>
    /// The speed with which the volume fades in/out. 0.001f is reeeeaaally slow, 1.0f is instant
    /// </summary>
    [Range(0.001f, 1.0f)]
    public float incrementVolume = 0.1f;

    private AudioSource _source;
    private bool _shouldIncreaseVolume;

    public bool shouldIncreaseVolume
    {
        set { _shouldIncreaseVolume = value; }
        get { return _shouldIncreaseVolume; }
    }

    // Use this for initialization
    private void Start () {
        _source = gameObject.GetComponent<AudioSource>();
	}

    // Update is called once per frame
    private void FixedUpdate () {
        ChangeVolume();
    }

    private void ChangeVolume()
    {
        if (_shouldIncreaseVolume)
        {
            if (!_source.isPlaying)
                _source.Play();
            if (_source.volume < 1.0f)
                _source.volume += incrementVolume;
            if (_source.volume > 1.0f)
                _source.volume = 1.0f;
        }
        else if (_source.volume > 0.0f)
        {
            _source.volume -= incrementVolume;
            if (_source.volume <= 0.0f)
            {
                _source.volume = 0.0f;
                if (_source.isPlaying)
                    _source.Stop();
            }
        }
    }
}
