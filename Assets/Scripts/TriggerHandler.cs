using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour {

    public KeyCode InteractionKey;

    public bool IsLightPlayer;

    private PlayerMovement _playerMovement;
    private PlayerStats _playerStats;

    private GameObject _nearestHidingSpot;

    private bool _isInDarkArea = true;
    private bool _isHiding = false;
    private bool _inButtonRange = false;
    private bool _inHidingRange = false;

	private void Awake () {
        _playerStats = this.gameObject.GetComponent<PlayerStats>();
        _playerMovement = this.gameObject.GetComponent<PlayerMovement>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Button")){
            _inButtonRange = true;
        }

        if (other.gameObject.CompareTag("HidingSpot")){
            _inHidingRange = true;
            _nearestHidingSpot = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other){
        if (IsLightPlayer){
            _isInDarkArea = false;
        }
        else {
            _playerStats.PlayerHealth -= 0.1f;
        }
    }

    private void OnTriggerExit(Collider other){
        if (IsLightPlayer){
            _isInDarkArea = true;
        }

        if (other.gameObject.CompareTag("Button")){
            _inButtonRange = false;
        }

        if (other.gameObject.CompareTag("HidingSpot")){
            _inHidingRange = false;
        }
    }

    private void FixedUpdate () {

        if (IsLightPlayer){
            if (_isInDarkArea){
                _playerStats.PlayerHealth -= 0.1f;
            }
        }

        if (Input.GetKeyDown(InteractionKey)){
            if (_isHiding){
                _playerMovement.StopHiding();
                _isHiding = false;
                _inHidingRange = false;
                return;
            }

            if (_inButtonRange){
                print("Button Pressed");
            }

            if (_inHidingRange)
            {
                if (IsLightPlayer){
                    _playerMovement.HideAt(_nearestHidingSpot.GetComponent<HidingAnchor>().LightLocation);
                }
                else{
                    _playerMovement.HideAt(_nearestHidingSpot.GetComponent<HidingAnchor>().DarkLocation);
                }
                _isHiding = true;
            }
        }
    }
}
