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

    private KeyCode _ctrlInteractKey = KeyCode.JoystickButton1; //X Button on the PS4 controller, might be changed, but probably not due to unofficial button mapping standardization;

    private ButtonSelect[] _buttons = new ButtonSelect[4];

    private ButtonSelect _exitYes;
    private ButtonSelect _exitNo;
    private ButtonSelect _creditsBack;

    private float _ctrlCooldown;
    private int _currentSelected;

    private bool _lockMenu = false;
    private bool _lockSelection = false;

    private void OnEnable()
    {
        foreach (ButtonSelect buttonSelect in mainScreen.gameObject.GetComponentsInChildren<ButtonSelect>())
        {
            switch (buttonSelect.gameObject.name)
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

        _buttons[0].State = ButtonStates.Selected;
        _currentSelected = 0;

        _ctrlCooldown = Time.time + 0.2f;
    }

    private void ResumeGame()
    {
        SceneManager.LoadSceneAsync(1); //Level
        gameObject.SetActive(false);
        print("NOTE: Continue is Virtually Identical to New Game"); //TODO
    }

    private void NewGame()
    {
        SceneManager.LoadSceneAsync(4); //ComicStrips
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

        if ((Input.GetAxis("CADPY") > 0 && _ctrlCooldown < Time.time) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _buttons[_currentSelected].State--;

            if (_currentSelected == 3)
                _currentSelected = 0;
            else
                _currentSelected += 1;

            _buttons[_currentSelected].State++;
            _ctrlCooldown = Time.time + 0.2f;
        }

        if ((Input.GetAxis("CADPY") < 0 && _ctrlCooldown < Time.time) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _buttons[_currentSelected].State--;

            if (_currentSelected == 0)
                _currentSelected = 3;
            else
                _currentSelected -= 1;

            _buttons[_currentSelected].State++;
            _ctrlCooldown = Time.time + 0.2f;
        }
    }

    private void OptionSelection()
    {
        if (_lockSelection) return;
        
        if (Input.GetKeyDown(KeyCode.Escape))
            QuitGame();

        if (Input.GetKey(_ctrlInteractKey) || Input.GetKey(interactKeyLP) || Input.GetKey(KeyCode.Return))
        {
            _buttons[_currentSelected].State = ButtonStates.Pressed;
            _lockMenu = true;
        }

        if (Input.GetKeyUp(_ctrlInteractKey) || Input.GetKeyUp(interactKeyLP) || Input.GetKeyUp(KeyCode.Return))
        {
            _buttons[_currentSelected].State = ButtonStates.Normal;

            switch (_currentSelected)
            {
                case 0: //Continue
                    _lockSelection = true;
                    ResumeGame();
                    break;

                case 1: //NewGame
                    _lockSelection = true;
                    NewGame();
                    break;

                case 2: //Credits
                    _lockSelection = true;
                    mainScreen.gameObject.SetActive(false);
                    creditsScreen.gameObject.SetActive(true);
                    _creditsBack = creditsScreen.gameObject.GetComponentInChildren<ButtonSelect>();
                    _creditsBack.State = ButtonStates.Selected;
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
                    _exitNo.State = ButtonStates.Selected;
                    break;

                default:
                    print("SubMenu not found. Did you assign and name them properly?");
                    break;
            }
        }
    }

    private void Update()
    {
        MenuNavigation();
        OptionSelection();

        if (exitCheck.gameObject.activeSelf)
            ExitCheckSelection();

        if (creditsScreen.gameObject.activeSelf)
            CreditsScreenSelection();
    }

    private void CreditsScreenSelection()
    {
        if (Input.GetKey(_ctrlInteractKey) || Input.GetKey(interactKeyLP) || Input.GetKey(KeyCode.Return))
            _creditsBack.State = ButtonStates.Pressed;

        if ((Input.GetKeyUp(_ctrlInteractKey) || Input.GetKeyUp(interactKeyLP) || Input.GetKeyUp(KeyCode.Return)) && _creditsBack.State == ButtonStates.Pressed)
        {
            _lockMenu = false;
            _lockSelection = false;
            _buttons[0].State = ButtonStates.Selected;
            _currentSelected = 0;
            creditsScreen.gameObject.SetActive(false);
            mainScreen.gameObject.SetActive(true);
        }
    }

    private void ExitCheckSelection()
    {
        if ((Input.GetAxis("CADPY") > 0 && _ctrlCooldown < Time.realtimeSinceStartup) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _exitYes.State = ButtonStates.Normal;
            _exitNo.State = ButtonStates.Selected;
            _ctrlCooldown = Time.realtimeSinceStartup + 0.2f;
        }

        if ((Input.GetAxis("CADPY") < 0 && _ctrlCooldown < Time.realtimeSinceStartup) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _exitYes.State = ButtonStates.Selected;
            _exitNo.State = ButtonStates.Normal;
            _ctrlCooldown = Time.realtimeSinceStartup + 0.2f;
        }

        if (Input.GetKey(_ctrlInteractKey) || Input.GetKey(interactKeyLP) || Input.GetKey(interactKeyDP))
        {
            if (_exitYes.State == ButtonStates.Selected)
                _exitYes.State = ButtonStates.Pressed;
            else if (_exitNo.State == ButtonStates.Selected)
                _exitNo.State = ButtonStates.Pressed;
        }

        if (Input.GetKeyUp(_ctrlInteractKey) || Input.GetKeyUp(interactKeyLP) || Input.GetKeyUp(interactKeyDP))
        {
            if (_exitYes.State == ButtonStates.Pressed)
            {
                _exitYes.State = ButtonStates.Normal;
                exitCheck.gameObject.SetActive(false);
                QuitGame();
            }
            else if (_exitNo.State == ButtonStates.Pressed)
            {
                _exitNo.State = ButtonStates.Normal;
                _lockMenu = false;
                _lockSelection = false;
                _buttons[0].State = ButtonStates.Selected;
                _currentSelected = 0;
                exitCheck.gameObject.SetActive(false);
            }
        }
    }
}