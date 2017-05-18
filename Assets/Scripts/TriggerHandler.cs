using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TriggerHandler : MonoBehaviour
{
    public KeyCode interactionKey;
    public KeyCode alternateInteractionKey;

    public Image interactPopUp;
    public Image hidePopUp;

    public float damageSpeed = 0.1f;
    public int mostRecentCheckpoint;
    public bool isLightPlayer;

    private PlayerMovement _playerMovement;
    private PlayerStats _playerStats;

    private GameObject _nearestInteractable;

    private Vector3 _mostRecentCheckpointPos;
    private Vector3 _mostRecentCheckpointRot;

    private bool _isInDarkArea = true;
    private bool _isHiding = false;
    private bool _inButtonRange = false;
    private bool _inHidingRange = false;
    private bool _inNoteRange = false;
    private bool _inCarShadow = false;
    private bool _inDoorRange = false;

    private void Awake()
    {
        _playerStats = this.gameObject.GetComponent<PlayerStats>();
        _playerMovement = this.gameObject.GetComponent<PlayerMovement>();
        _mostRecentCheckpointPos = transform.position;
    }

    public Vector3 mostRecentCheckpointPos
    {
        get { return _mostRecentCheckpointPos; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("AudioSource"))
        {
            other.GetComponent<AudioBoxScript>().shouldIncreaseVolume = true;
        }
        if (other.gameObject.CompareTag("Light"))
        {
            _isInDarkArea = false;
        }
        if (other.gameObject.CompareTag("Button"))
        {
            _inButtonRange = true;
            _nearestInteractable = other.gameObject;
            interactPopUp.gameObject.SetActive(true);
        }
        if (other.gameObject.CompareTag("HidingSpot"))
        {
            _inHidingRange = true;
            _nearestInteractable = other.gameObject;
            hidePopUp.gameObject.SetActive(true);
        }
        if(other.gameObject.CompareTag("Door"))
        {
            _inDoorRange = true;
            _nearestInteractable = other.gameObject;
            interactPopUp.gameObject.SetActive(true);
        }
        if (other.gameObject.CompareTag("Note"))
        {
            if (other.gameObject.GetComponent<NoteScript>().instantCollect)
            {
                _nearestInteractable = other.gameObject;
                CollectNote();
            }
            else
            {
                _inNoteRange = true;
                _nearestInteractable = other.gameObject;
                interactPopUp.gameObject.SetActive(true);
            }
        }
        if(other.gameObject.CompareTag("CheckPoint"))
        {
            CheckPoint checkpoint = other.gameObject.GetComponent<CheckPoint>();
            if (mostRecentCheckpoint < checkpoint.index || checkpoint.returnAble)
            {
                mostRecentCheckpoint = checkpoint.index;

                if (isLightPlayer)
                {
                    _mostRecentCheckpointPos = checkpoint.LightLocation;
                    _mostRecentCheckpointRot = checkpoint.LightRotation;
                }
                else
                {
                    _mostRecentCheckpointPos = checkpoint.DarkLocation;
                    _mostRecentCheckpointRot = checkpoint.DarkRotation;
                }
            }
        }
        if(other.gameObject.CompareTag("Artifact"))
        {
            Win();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Light"))
        {
            _isInDarkArea = false;
        }
        if (other.gameObject.CompareTag("CarShadow"))
        {
            _inCarShadow = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Light"))
        {
            _isInDarkArea = true;
        }
        if (other.gameObject.CompareTag("Button"))
        {
            _inButtonRange = false;
            _nearestInteractable = null;
            interactPopUp.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("HidingSpot"))
        {
            _inHidingRange = false;
            _nearestInteractable = null;
            hidePopUp.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Note"))
        {
            _inNoteRange = false;
            _nearestInteractable = null;
            interactPopUp.gameObject.SetActive(false);
        }
        if(other.gameObject.CompareTag("Door"))
        {
            _inDoorRange = false;
            _nearestInteractable = null;
            interactPopUp.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("AudioSource"))
        {
            other.GetComponent<AudioBoxScript>().shouldIncreaseVolume = false;
        }
    }

    /// For the sake of both of our sanity: Please don't fill up the Fixed Update with random lines, just put them in a function if you have to. 
    /// You know, these things the teachers said about making it clear what you're trying to do so others don't have to spend an hour sifting through your work to see what's happening
    private void FixedUpdate()
    {
        AffectPlayer();
        GetInput();
        PostUpdate();
    }

    private void Win()
    {
        if (isLightPlayer)
            SceneManager.LoadScene(2); //Light Ending (Ending_Comic_Strips)
        else
            SceneManager.LoadScene(3); //Dark Ending
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(interactionKey) || Input.GetKeyDown(alternateInteractionKey))
        {
            if(_playerStats.GameOver)
            {
                _playerStats.Respawn(_mostRecentCheckpointPos, _mostRecentCheckpointRot);
                _playerMovement.Reset();

                interactPopUp.gameObject.SetActive(false);

                return;
            }
            if (_isHiding)
            {
                _playerMovement.StopHiding();
                _isHiding = false;
                _playerMovement.canMove = true;
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                AffectPlayer();
                return;
            }
            if (_inButtonRange && _nearestInteractable.GetComponent<LightSwitch>().switchType != TypeOfSwitch.holdToggle)
            {
                _nearestInteractable.GetComponent<LightSwitch>().Toggle();
                return;
            }
            if (_inNoteRange)
            {
                CollectNote();
                return;
            }
            if (_inHidingRange)
            {
                if (isLightPlayer)
                {
                    _playerMovement.HideAt(_nearestInteractable.GetComponent<HidingAnchor>().LightLocation);
                }
                else
                {
                    _playerMovement.HideAt(_nearestInteractable.GetComponent<HidingAnchor>().DarkLocation);
                }
                _isHiding = true;
                _playerMovement.canMove = false;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                return;
            }
            if(_inDoorRange)
            {
                _nearestInteractable.GetComponent<Animator>().SetBool("ShouldOpen", true);
                _nearestInteractable.GetComponent<BoxCollider>().enabled = false;
                _nearestInteractable.GetComponent<DoorSounds>().PlayDoorOpenSound();
                interactPopUp.gameObject.SetActive(false);
                _inDoorRange = false;
                _nearestInteractable = null;
                return;
            }
        }
        if (_inButtonRange && _nearestInteractable.GetComponent<LightSwitch>() != null && _nearestInteractable.GetComponent<LightSwitch>().switchType == TypeOfSwitch.holdToggle && (Input.GetKey(interactionKey) || Input.GetKey(alternateInteractionKey)))
        {
            _nearestInteractable.GetComponent<LightSwitch>().Toggle();
        }
    }

    /// <summary>
    /// Hurts the player if they're outside of their element, regenerates otherwise
    /// </summary>
    private void AffectPlayer()
    {

        if (_playerStats.GameOver)
        {
            //interactPopUp.gameObject.SetActive(true);
            return;
        }

        if (_isHiding)
        {
            if(_playerStats.PlayerHealth < _playerStats.MaxHealth)
            _playerStats.PlayerHealth += damageSpeed / 2;
            return;
        }
        if (isLightPlayer)
        {
            if ((_isInDarkArea || _inCarShadow) && _playerStats.PlayerHealth > 0)
                _playerStats.PlayerHealth -= damageSpeed;
            else if (_playerStats.PlayerHealth > 0 && _playerStats.PlayerHealth < _playerStats.MaxHealth)
                _playerStats.PlayerHealth += damageSpeed / 2;
        }
        else
        {
            if ((!_isInDarkArea && !_inCarShadow) && _playerStats.PlayerHealth > 0)
                _playerStats.PlayerHealth -= damageSpeed;
            else if (_playerStats.PlayerHealth > 0 && _playerStats.PlayerHealth < _playerStats.MaxHealth)
                _playerStats.PlayerHealth += damageSpeed / 2;
        }
    }

    private void PostUpdate()
    {
        _isInDarkArea = true;
        _inCarShadow = false;
    }

    /// <summary>
    /// Collects _nearestInteractable. Will give errors if _nearestInteractable does not have a NoteScript, so ensure it's used
    /// </summary>
    private void CollectNote()
    {
        if (_nearestInteractable.GetComponent<NoteScript>().NoteID == 404)
        {
            print("! NOTE HAS NOT BEEN ASSIGNED AN ID !");
            return;
        }
        _playerStats.NoteData[_nearestInteractable.GetComponent<NoteScript>().NoteID] = _nearestInteractable.GetComponent<NoteScript>().NoteText;
        Destroy(_nearestInteractable.gameObject);
        //print("Note" + _nearestInteractable.GetComponent<NoteScript>().NoteID + " Collected, which contained: " + _playerStats.NoteData[_nearestInteractable.GetComponent<NoteScript>().NoteID]);
        _nearestInteractable = null;
    }
}
