using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public KeyCode interactKeyLP = KeyCode.E; //Default Interaction KeyCode for the Light Player.
    public KeyCode interactKeyDP = KeyCode.Backslash; //Default Interaction KeyCode for the Dark Player.

    public Canvas mainScreen;
    public Canvas exitCheck;
    public Canvas creditsScreen;

    private KeyCode _pauseKey = KeyCode.P; //Default KeyCode, should be changed to the correct one by PlayerMovement.
    private KeyCode _ctrlPauseKey = KeyCode.JoystickButton9; //Options Button on the PS4 controller, won't be changed.
    private KeyCode _ctrlInteractKey = KeyCode.JoystickButton1; //X Button on the PS4 controller, might be changed, but probably not due to unofficial button mapping standardization;

    private ButtonSelect[] _buttons = new ButtonSelect[4];

    private ButtonSelect _exitYes;
    private ButtonSelect _exitNo;
    private ButtonSelect _creditsBack;

    private float _pauseCooldown;
    private float _ctrlCooldown;
    private int _currentSelected;

    private bool _lockMenu = false;
    private bool _lockSelection = false;

    private void OnEnable()
    {
        foreach (ButtonSelect buttonSelect in mainScreen.gameObject.GetComponentsInChildren<ButtonSelect>())
        {
            string[] splitName = buttonSelect.gameObject.name.Split(' ');
            switch (splitName[0])
            {
                case "Continue":
                    _buttons[0] = buttonSelect;
                    break;
                case "NewGame":
                    _buttons[1] = buttonSelect;
                    break;
                case "Credits":
                    _buttons[2] = buttonSelect;
                    break;
                case "Quit":
                    _buttons[3] = buttonSelect;
                    break;
                default:
                    print("Button Not Found. Did you name them properly?");
                    break;
            }
        }

        _buttons[0].State = ButtonSelect.ButtonStates.Selected;
        _currentSelected = 0;

        _pauseCooldown = Time.time + 0.1f;
        _ctrlCooldown = Time.time + 0.2f;
    }

    public KeyCode PauseKey
    {
        set { _pauseKey = value; }
        get { return _pauseKey; }
    }

    private void ResumeGame()
    {
        SceneManager.LoadSceneAsync("Level");
        gameObject.SetActive(false);
        print("NOTE: Continue is Virtually Identical to New Game"); //TODO
    }

    private void NewGame()
    {
        SceneManager.LoadSceneAsync("ComicStrips");
        gameObject.SetActive(false);
        print("NOTE: New Game is Virtually Identical to Continue"); //TODO
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

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxis("CADPY") > 0 && _ctrlCooldown < Time.time))
        {
            _buttons[_currentSelected].State = ButtonSelect.ButtonStates.Normal;

            if (_currentSelected == 3)
                _currentSelected = 0;
            else
                _currentSelected += 1;

            _buttons[_currentSelected].State = ButtonSelect.ButtonStates.Selected;
            _ctrlCooldown = Time.time + 0.2f;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxis("CADPY") < 0 && _ctrlCooldown < Time.time))
        {
            _buttons[_currentSelected].State = ButtonSelect.ButtonStates.Normal;

            if (_currentSelected == 0)
                _currentSelected = 3;
            else
                _currentSelected -= 1;

            _buttons[_currentSelected].State = ButtonSelect.ButtonStates.Selected;
            _ctrlCooldown = Time.time + 0.2f;
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

                case 1: //NewGame
                    NewGame();
                    break;

                case 2: //Credits
                    _lockSelection = true;
                    mainScreen.gameObject.SetActive(false);
                    creditsScreen.gameObject.SetActive(true);
                    _creditsBack = creditsScreen.gameObject.GetComponentInChildren<ButtonSelect>();
                    _creditsBack.State = ButtonSelect.ButtonStates.Selected;
                    break;

                case 3: //Quit
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

    private void Update()
    {
        if (_pauseCooldown > Time.time) return;

        MenuNavigation();
        OptionSelection();

        if (exitCheck.gameObject.activeSelf)
            ExitCheckSelection();

        if (creditsScreen.gameObject.activeSelf)
            CreditsScreenSelection();
    }

    private void CreditsScreenSelection()
    {
        if (Input.GetKey(interactKeyLP) || Input.GetKey(interactKeyDP) || Input.GetKey(_ctrlInteractKey))
            _creditsBack.State = ButtonSelect.ButtonStates.Pressed;

        if ((Input.GetKeyUp(interactKeyLP) || Input.GetKeyUp(interactKeyDP) || Input.GetKeyUp(_ctrlInteractKey)) && _creditsBack.State == ButtonSelect.ButtonStates.Pressed)
        {
            _lockMenu = false;
            _lockSelection = false;
            _buttons[0].State = ButtonSelect.ButtonStates.Selected;
            _currentSelected = 0;
            creditsScreen.gameObject.SetActive(false);
            mainScreen.gameObject.SetActive(true);
        }
    }

    private void ExitCheckSelection()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxis("CADPY") > 0 && _ctrlCooldown < Time.time))
        {
            _exitYes.State = ButtonSelect.ButtonStates.Normal;
            _exitNo.State = ButtonSelect.ButtonStates.Selected;
            _ctrlCooldown = Time.time + 0.2f;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxis("CADPY") < 0 && _ctrlCooldown < Time.time))
        {
            _exitYes.State = ButtonSelect.ButtonStates.Selected;
            _exitNo.State = ButtonSelect.ButtonStates.Normal;
            _ctrlCooldown = Time.time + 0.2f;
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
