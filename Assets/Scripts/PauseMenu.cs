using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    private KeyCode _pauseKey = KeyCode.P; //Default KeyCode, should be changed to the correct one by PlayerMovement

    private float _pauseCooldown;

	private void OnEnable ()
    {
        print("gets called: PauseMenu OnEnable()");
        Time.timeScale = 0;
        _pauseCooldown = Time.realtimeSinceStartup + 0.1f;
    }

    private void OnDisable()
    {
        print("gets called: PauseMenu OnDisable()");
        Time.timeScale = 1;
    }

    public KeyCode PauseKey
    {
        set { _pauseKey = value; }
        get { return _pauseKey; }
    }
	
	private void Update () {
        if (_pauseCooldown > Time.realtimeSinceStartup) return;

        print("gets called: PauseMenu Update()");

        if (Input.GetKeyDown(_pauseKey))
        {
            print("gets called: PauseMenu GetKeyDown");
            gameObject.SetActive(false);
        }
	}
}
