using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public KeyCode interactKeyLP = KeyCode.E; //Default Interaction KeyCode for the Light Player.
    public KeyCode interactKeyDP = KeyCode.Backslash; //Default Interaction KeyCode for the Dark Player.

    public Canvas exitCheck;
    public Canvas controlsScreen;
    public Canvas audioScreen;

    private KeyCode _pauseKey = KeyCode.P; //Default KeyCode, should be changed to the correct one by PlayerMovement.
    private KeyCode _ctrlPauseKey = KeyCode.JoystickButton9; //Options Button on the PS4 controller, won't be changed.
    private KeyCode _ctrlInteractKey = KeyCode.JoystickButton1; //X Button on the PS4 controller, might be changed, but probably not due to unofficial button mapping standardization;

    private ButtonSelect[] _buttons = new ButtonSelect[4];

    private ButtonSelect _exitYes;
    private ButtonSelect _exitNo;
    private ButtonSelect _controlsBack;
    private ButtonSelect _audioBack;

    private SliderSelect _soundSlider;
    private SliderSelect _musicSlider;

    private float _pauseCooldown;
    private float _ctrlCooldown;
    private int _currentSelected;

    private bool _lockMenu = false;
    private bool _lockSelection = false;

	private void OnEnable ()
    {
        foreach (ButtonSelect buttonSelect in gameObject.GetComponentsInChildren<ButtonSelect>())
        {
            string[] splitName = buttonSelect.gameObject.name.Split(' ');
            switch (splitName[0])
            {
                case "Continue":
                    _buttons[0] = buttonSelect;
                    break;
                case "Controls":
                    _buttons[1] = buttonSelect;
                    break;
                case "Music":
                    _buttons[2] = buttonSelect;
                    break;
                case "Exit":
                    _buttons[3] = buttonSelect;
                    break;
                default:
                    print("Button Not Found. Did you name them properly?");
                    break;
            }
        }

        _buttons[0].State = ButtonSelect.ButtonStates.Selected;
        _currentSelected = 0;

        Time.timeScale = 0;
        _pauseCooldown = Time.realtimeSinceStartup + 0.1f;
        _ctrlCooldown = Time.realtimeSinceStartup + 0.2f;
    }

    private void OnDisable()
    {
        Time.timeScale = 0;
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
        if (_lockMenu) return;

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxis("CADPY") > 0 && _ctrlCooldown < Time.realtimeSinceStartup))
        {
            _buttons[_currentSelected].State = ButtonSelect.ButtonStates.Normal;

            if (_currentSelected == 3)
                _currentSelected = 0;
            else
                _currentSelected += 1;

            _buttons[_currentSelected].State = ButtonSelect.ButtonStates.Selected;
            _ctrlCooldown = Time.realtimeSinceStartup + 0.2f;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxis("CADPY") < 0 && _ctrlCooldown < Time.realtimeSinceStartup))
        {
            _buttons[_currentSelected].State = ButtonSelect.ButtonStates.Normal;

            if (_currentSelected == 0)
                _currentSelected = 3;
            else
                _currentSelected -= 1;

            _buttons[_currentSelected].State = ButtonSelect.ButtonStates.Selected;
            _ctrlCooldown = Time.realtimeSinceStartup + 0.2f;
        }
    }

    private void OptionSelection()
    {
        if (Input.GetKeyDown(_pauseKey) || Input.GetKeyDown(_ctrlPauseKey))
            ResumeGame();

        if (_lockSelection) return;

        if (Input.GetKey(interactKeyLP) || Input.GetKey(interactKeyDP) || Input.GetKey(_ctrlInteractKey))
        {
            _buttons[_currentSelected].State = ButtonSelect.ButtonStates.Pressed;
            _lockMenu = true;
        }

        if (Input.GetKeyUp(interactKeyLP) || Input.GetKeyUp(interactKeyDP) || Input.GetKeyUp(_ctrlInteractKey))
        {
            _buttons[_currentSelected].State = ButtonSelect.ButtonStates.Normal;

            switch (_currentSelected)
            {
                case 0: //Continue
                    ResumeGame();
                    break;

                case 1: //Controls
                    _lockSelection = true;
                    controlsScreen.gameObject.SetActive(true);
                    _controlsBack = controlsScreen.gameObject.GetComponentInChildren<ButtonSelect>();
                    _controlsBack.State = ButtonSelect.ButtonStates.Selected;
                    break;

                case 2: //Music
                    _lockSelection = true;
                    audioScreen.gameObject.SetActive(true);
                    _audioBack = audioScreen.gameObject.GetComponentInChildren<ButtonSelect>();
                    _audioBack.State = ButtonSelect.ButtonStates.Selected;

                    foreach(SliderSelect sliderSelect in audioScreen.gameObject.GetComponentsInChildren<SliderSelect>())
                    {
                        switch (sliderSelect.gameObject.name)
                        {
                            case "Slider Music":
                                _musicSlider = sliderSelect;
                                break;
                            case "Slider Sound Effects":
                                _soundSlider = sliderSelect;
                                break;
                            default:
                                print("Slider Not Found. Did you name them properly?");
                                break;
                        }
                    }
                    break;

                case 3: //Exit
                    _lockSelection = true;
                    exitCheck.gameObject.SetActive(true);

                    foreach (ButtonSelect buttonSelect in exitCheck.gameObject.GetComponentsInChildren<ButtonSelect>())
                    {
                        switch (buttonSelect.gameObject.name)
                        {
                            case "yes":
                                _exitYes = buttonSelect;
                                break;
                            case "no":
                                _exitNo = buttonSelect;
                                break;
                            default:
                                print("Button Not Found. Did you name them properly?");
                                break;
                        }
                    }
                    _exitNo.State = ButtonSelect.ButtonStates.Selected;
                    break;

                default:
                    print("SubMenu not found. Did you assign and name them properly?");
                    break;
            }
        }
    }
	
	private void Update () {
        if (_pauseCooldown > Time.realtimeSinceStartup) return;

        MenuNavigation();
        OptionSelection();

        if (exitCheck.gameObject.activeSelf)
            ExitCheckSelection();

        if (controlsScreen.gameObject.activeSelf)
            ControlsScreenSelection();

        if (audioScreen.gameObject.activeSelf)
            audioScreenSelection();
	}

    private void ControlsScreenSelection()
    {
        if (Input.GetKey(interactKeyLP) || Input.GetKey(interactKeyDP) || Input.GetKey(_ctrlInteractKey))
            _controlsBack.State = ButtonSelect.ButtonStates.Pressed;

        if ((Input.GetKeyUp(interactKeyLP) || Input.GetKeyUp(interactKeyDP) || Input.GetKeyUp(_ctrlInteractKey)) && _controlsBack.State == ButtonSelect.ButtonStates.Pressed)
        {
            _lockMenu = false;
            _lockSelection = false;
            _buttons[0].State = ButtonSelect.ButtonStates.Selected;
            _currentSelected = 0;
            controlsScreen.gameObject.SetActive(false);
        }
    }

    private void audioScreenSelection()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxis("CADPY") > 0 && _ctrlCooldown < Time.realtimeSinceStartup))
        {
            if (_audioBack.State == ButtonSelect.ButtonStates.Selected)
            {
                _audioBack.State = ButtonSelect.ButtonStates.Normal;
                _soundSlider.State = ButtonSelect.ButtonStates.Selected;
            }
            else if (_musicSlider.State == ButtonSelect.ButtonStates.Selected)
            {
                _musicSlider.State = ButtonSelect.ButtonStates.Normal;
                _audioBack.State = ButtonSelect.ButtonStates.Selected;
            }
            else if (_soundSlider.State == ButtonSelect.ButtonStates.Selected)
            {
                _soundSlider.State = ButtonSelect.ButtonStates.Normal;
                _musicSlider.State = ButtonSelect.ButtonStates.Selected;
            }
            _ctrlCooldown = Time.realtimeSinceStartup + 0.2f;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxis("CADPY") < 0 && _ctrlCooldown < Time.realtimeSinceStartup))
        {
            if (_audioBack.State == ButtonSelect.ButtonStates.Selected)
            {
                _audioBack.State = ButtonSelect.ButtonStates.Normal;
                _musicSlider.State = ButtonSelect.ButtonStates.Selected;
            }
            else if (_musicSlider.State == ButtonSelect.ButtonStates.Selected)
            {
                _musicSlider.State = ButtonSelect.ButtonStates.Normal;
                _soundSlider.State = ButtonSelect.ButtonStates.Selected;
            }
            else if(_soundSlider.State == ButtonSelect.ButtonStates.Selected)
            {
                _soundSlider.State = ButtonSelect.ButtonStates.Normal;
                _audioBack.State = ButtonSelect.ButtonStates.Selected;
            }
            _ctrlCooldown = Time.realtimeSinceStartup + 0.2f;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("CADPX") > 0)
        {
            if (_musicSlider.State == ButtonSelect.ButtonStates.Selected && _musicSlider.value > 0.0f)
                _musicSlider.value -= 0.01f;
            else if (_soundSlider.State == ButtonSelect.ButtonStates.Selected && _soundSlider.value > 0.0f)
                _soundSlider.value -= 0.01f;
        }
        else if (Input.GetAxis("CALSX") > 0)
        {
            if (_musicSlider.State == ButtonSelect.ButtonStates.Selected && _musicSlider.value > 0.0f)
                _musicSlider.value -= (Input.GetAxis("CALSX") * 0.02f);
            else if (_soundSlider.State == ButtonSelect.ButtonStates.Selected && _soundSlider.value > 0.0f)
                _soundSlider.value -= (Input.GetAxis("CALSX") * 0.02f);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("CADPX") < 0)
        {
            if (_musicSlider.State == ButtonSelect.ButtonStates.Selected && _musicSlider.value < 1.0f)
                _musicSlider.value += 0.01f;
            else if (_soundSlider.State == ButtonSelect.ButtonStates.Selected && _soundSlider.value < 1.0f)
                _soundSlider.value += 0.01f;
        }
        else if (Input.GetAxis("CALSX") < 0)
        {
            if (_musicSlider.State == ButtonSelect.ButtonStates.Selected && _musicSlider.value < 1.0f)
                _musicSlider.value -= (Input.GetAxis("CALSX") * 0.02f);
            else if (_soundSlider.State == ButtonSelect.ButtonStates.Selected && _soundSlider.value < 1.0f)
                _soundSlider.value -= (Input.GetAxis("CALSX") * 0.02f);
        }

        if (Input.GetKey(interactKeyLP) || Input.GetKey(interactKeyDP) || Input.GetKey(_ctrlInteractKey))
            if (_audioBack.State == ButtonSelect.ButtonStates.Selected)
                _audioBack.State = ButtonSelect.ButtonStates.Pressed;

        if (Input.GetKeyUp(interactKeyLP) || Input.GetKeyUp(interactKeyDP) || Input.GetKeyUp(_ctrlInteractKey))
        {
            if (_audioBack.State == ButtonSelect.ButtonStates.Pressed)
            {
                PlayerPrefs.SetFloat("MusicVolume", _musicSlider.value);
                PlayerPrefs.SetFloat("SFXVolume", _soundSlider.value);
                PlayerPrefs.Save();
                _lockMenu = false;
                _lockSelection = false;
                _buttons[0].State = ButtonSelect.ButtonStates.Selected;
                _currentSelected = 0;
                audioScreen.gameObject.SetActive(false);
            }
        }
    }

    private void ExitCheckSelection()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxis("CADPY") > 0 && _ctrlCooldown < Time.realtimeSinceStartup))
        {
            _exitYes.State = ButtonSelect.ButtonStates.Normal;
            _exitNo.State = ButtonSelect.ButtonStates.Selected;
            _ctrlCooldown = Time.realtimeSinceStartup + 0.2f;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxis("CADPY") < 0 && _ctrlCooldown < Time.realtimeSinceStartup))
        {
            _exitYes.State = ButtonSelect.ButtonStates.Selected;
            _exitNo.State = ButtonSelect.ButtonStates.Normal;
            _ctrlCooldown = Time.realtimeSinceStartup + 0.2f;
        }

        if (Input.GetKey(interactKeyLP) || Input.GetKey(interactKeyDP) || Input.GetKey(_ctrlInteractKey))
        {
            if (_exitYes.State == ButtonSelect.ButtonStates.Selected)
                _exitYes.State = ButtonSelect.ButtonStates.Pressed;
            else if (_exitNo.State == ButtonSelect.ButtonStates.Selected)
                _exitNo.State = ButtonSelect.ButtonStates.Pressed;
        }

        if (Input.GetKeyUp(interactKeyLP) || Input.GetKeyUp(interactKeyDP) || Input.GetKeyUp(_ctrlInteractKey))
        {
            if (_exitYes.State == ButtonSelect.ButtonStates.Pressed)
            {
                _exitYes.State = ButtonSelect.ButtonStates.Normal;
                exitCheck.gameObject.SetActive(false);
                QuitGame();
            }
            else if (_exitNo.State == ButtonSelect.ButtonStates.Pressed)
            {
                _exitNo.State = ButtonSelect.ButtonStates.Normal;
                _lockMenu = false;
                _lockSelection = false;
                _buttons[0].State = ButtonSelect.ButtonStates.Selected;
                _currentSelected = 0;
                exitCheck.gameObject.SetActive(false);
            }
        }       
    }
}
