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

    private Image[] _buttons = new Image[4];

    //private Image _continue;
    //private Image _controls;
    //private Image _music;
    //private Image _quit;

    private float _pauseCooldown;

    private int _currentSelected;

	private void OnEnable ()
    {
        foreach (Image buttonImage in gameObject.GetComponentsInChildren<Image>())
        {
            string[] splitName = buttonImage.gameObject.name.Split(' ');
            switch (splitName[0])
            {
                case "Continue":
                    _buttons[0] = buttonImage;
                    break;
                case "Controls":
                    _buttons[1] = buttonImage;
                    break;
                case "Music":
                    _buttons[2] = buttonImage;
                    break;
                case "Exit":
                    _buttons[3] = buttonImage;
                    break;
                default:
                    break;
            }
        }

        _buttons[0].gameObject.GetComponent<ButtonSelect>().Selected = true;
        _currentSelected = 0;

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
            _buttons[_currentSelected].gameObject.GetComponent<ButtonSelect>().Selected = false;

            if (_currentSelected == 4)
                _currentSelected = 0;
            else
                _currentSelected += 1;

            _buttons[_currentSelected].gameObject.GetComponent<ButtonSelect>().Selected = true;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("CADPY") < 0)
        {
            _buttons[_currentSelected].gameObject.GetComponent<ButtonSelect>().Selected = false;

            if (_currentSelected == 0)
                _currentSelected = 4;
            else
                _currentSelected -= 1;

            _buttons[_currentSelected].gameObject.GetComponent<ButtonSelect>().Selected = true;
        }
    }

    private void OptionSelection()
    {
        if (Input.GetKeyDown(_pauseKey) || Input.GetKeyDown(_ctrlPauseKey))
            ResumeGame();

        if (Input.GetKeyDown(_interactKeyLP) || Input.GetKeyDown(_interactKeyDP) || Input.GetKeyDown(_ctrlInteractKey))
        {
            switch (_currentSelected)
            {
                case 1: //Continue
                    ResumeGame();
                    break;

                case 2: //Controls
                    print("TODO: IMPLEMENT MUSIC MENU"); //TODO
                    break;

                case 3: //Music
                    print("TODO: IMPLEMENT MUSIC MENU"); //TODO
                    break;

                case 4: //Exit
                    QuitGame();
                    break;
            }

        }
    }
	
	private void Update () {
        if (_pauseCooldown > Time.realtimeSinceStartup) return;

        MenuNavigation();
        OptionSelection();
	}
}
