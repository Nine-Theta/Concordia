using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public KeyCode _pauseKey = KeyCode.P; //Default KeyCode, should be changed to the correct one by PlayerMovement.
    public KeyCode _interactKeyLP = KeyCode.E; //Default Interaction KeyCode for the Light Player.
    public KeyCode _interactKeyDP = KeyCode.Backslash; //Default Interaction KeyCode for the Dark Player.
    private KeyCode _ctrlPauseKey = KeyCode.JoystickButton9; //Options Button on the PS4 controller, won't be changed.
    private KeyCode _ctrlInteractKey = KeyCode.JoystickButton1; //X Button on the PS4 controller, might be changed, but probably not due to unofficial button mapping standardization;

    private Image _resume;
    private Image _quit;

    private float _pauseCooldown;

	private void OnEnable ()
    {
        foreach (Image buttonImage in gameObject.GetComponentsInChildren<Image>())
        {
            string[] splitName = buttonImage.gameObject.name.Split(' ');
            switch (splitName[0])
            {
                case "Resume":
                    _resume = buttonImage;
                    break;
                case "Quit":
                    _quit = buttonImage;
                    break;
            }
        }

        _resume.gameObject.GetComponent<ButtonSelect>().Selected = true;

        Time.timeScale = 0;
        _pauseCooldown = Time.realtimeSinceStartup + 0.1f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public KeyCode PauseKey
    {
        set { _pauseKey = value; }
        get { return _pauseKey; }
    }

    private void ResumeGame()
    {
        print("pressed");
        _resume.gameObject.GetComponent<ButtonSelect>().Selected = false;
        _quit.gameObject.GetComponent<ButtonSelect>().Selected = false;
        gameObject.SetActive(false);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void MenuNavigation()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("CADPY") > 0)
        {
            _resume.gameObject.GetComponent<ButtonSelect>().Selected = false;
            _quit.gameObject.GetComponent<ButtonSelect>().Selected = true;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("CADPY") < 0)
        {
            _resume.gameObject.GetComponent<ButtonSelect>().Selected = true;
            _quit.gameObject.GetComponent<ButtonSelect>().Selected = false;
        }
    }

    private void OptionSelection()
    {
        if (Input.GetKeyDown(_interactKeyLP) || Input.GetKeyDown(_interactKeyDP) || Input.GetKeyDown(_ctrlInteractKey))
        {
            if(_resume.gameObject.GetComponent<ButtonSelect>().Selected)
            {
                ResumeGame();
            }

            if(_quit.gameObject.GetComponent<ButtonSelect>().Selected)
            {
                QuitGame();
            }
        }

        if (Input.GetKeyDown(_pauseKey) || Input.GetKeyDown(_ctrlPauseKey))
        {
            ResumeGame();
        }
    }
	
	private void Update () {
        if (_pauseCooldown > Time.realtimeSinceStartup) return;

        MenuNavigation();
        OptionSelection();
	}
}
