using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour {

    public KeyCode InteractionKey;

    public bool IsLightPlayer;

    private PlayerMovement _playerMovement;
    private PlayerStats _playerStats;

    private bool _isInDarkArea = true;
    private bool _isHiding = false;

	private void Awake () {
        _playerStats = this.gameObject.GetComponent<PlayerStats>();
        _playerMovement = this.gameObject.GetComponent<PlayerMovement>();
	}

    private void OnTriggerStay(Collider other){
        if (_isHiding) return;

        if (IsLightPlayer){
            _isInDarkArea = false;
        }
        else {
            _playerStats.PlayerHealth -= 0.1f;
        }

        if (other.gameObject.CompareTag("Button"))
        {
            if (Input.GetKeyDown(InteractionKey)){
                print(this.gameObject.name + ": Button Pressed: " + other.gameObject.tag);
            }
        }

        if (other.gameObject.CompareTag("HidingSpot"))
        {
            if (Input.GetKeyDown(InteractionKey)){
                print(this.gameObject.name + ": Button Pressed: " + other.gameObject.tag);
                _playerMovement.HideAt(other.gameObject);
                _isHiding = true;
            }
        }

    }

    private void OnTriggerExit(Collider other){
        if (IsLightPlayer){
            _isInDarkArea = true;
        }
    }

    private void FixedUpdate () {

        if (IsLightPlayer){
            if (_isInDarkArea){
                _playerStats.PlayerHealth -= 0.1f;
            }
        }

        if (_isHiding){
            if (Input.GetKeyDown(InteractionKey)){
                _playerMovement.StopHiding();
                _isHiding = false;
            }
        }
    }
}
