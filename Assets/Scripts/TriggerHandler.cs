using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{

    public KeyCode interactionKey;
    public KeyCode alternateInteractionKey;

    public bool isLightPlayer;

    private PlayerMovement _playerMovement;
    private PlayerStats _playerStats;

    private GameObject _nearestInteractable;

    private bool _isInDarkArea = true;
    private bool _isHiding = false;
    private bool _inButtonRange = false;
    private bool _inHidingRange = false;

    private void Awake()
    {
        _playerStats = this.gameObject.GetComponent<PlayerStats>();
        _playerMovement = this.gameObject.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Light"))
        {
            _isInDarkArea = false;
        }

        if (other.gameObject.CompareTag("Button"))
        {
            _inButtonRange = true;
            _nearestInteractable = other.gameObject;
        }

        if (other.gameObject.CompareTag("HidingSpot"))
        {
            _inHidingRange = true;
            _nearestInteractable = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Light"))
        {
            _isInDarkArea = false;
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
        }

        if (other.gameObject.CompareTag("HidingSpot"))
        {
            _inHidingRange = false;
            _nearestInteractable = null;
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

    private void GetInput()
    {
        if (Input.GetKeyDown(interactionKey) || Input.GetKeyDown(alternateInteractionKey))
        {
            if (_isHiding)
            {
                _playerMovement.StopHiding();
                _isHiding = false;
                _playerMovement.canMove = true;
                return;
            }

            if (_inButtonRange)
            {
                _nearestInteractable.GetComponent<LightSwitch>().Toggle();
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
            }
        }
    }

    /// <summary>
    /// Hurts the player if they're outside of their element, regenerates otherwise
    /// </summary>
    private void AffectPlayer()
    {
        if (_isHiding)
            return;
        if (isLightPlayer)
        {
            if (_isInDarkArea && _playerStats.PlayerHealth > 0)
                _playerStats.PlayerHealth -= 0.1f;
            else if (_playerStats.PlayerHealth > 0 && _playerStats.PlayerHealth < _playerStats.MaxHealth)
                _playerStats.PlayerHealth += 0.05f;
        }
        else
        {
            if (!_isInDarkArea && _playerStats.PlayerHealth > 0)
                _playerStats.PlayerHealth -= 0.1f;
            else if (_playerStats.PlayerHealth > 0 && _playerStats.PlayerHealth < _playerStats.MaxHealth)
                _playerStats.PlayerHealth += 0.05f;
        }
    }

    private void PostUpdate()
    {
        _isInDarkArea = true;
    }
}
