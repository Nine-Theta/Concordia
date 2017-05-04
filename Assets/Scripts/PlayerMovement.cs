using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public KeyCode forwardKey;
    public KeyCode backwardKey;
    public KeyCode moveLeftKey;
    public KeyCode moveRightKey;
    public KeyCode crouchKey;
    
    public string controllerNumber = "1";  //Should be either 1 or 2.

    private TriggerHandler _triggerHandler;
    private Rigidbody _playerBody;
    private PlayerStats _playerStats;

    private Vector3 _preHidingPos;

    private bool _canMove = true;

    private void Start () {
        _triggerHandler = this.gameObject.GetComponent<TriggerHandler>();
        _playerBody = this.gameObject.GetComponent<Rigidbody>();
        _playerStats = this.gameObject.GetComponent<PlayerStats>();
	}

    public bool canMove{
        get { return _canMove; }
        set { _canMove = value; }
    }

    public void HideAt(GameObject HidingSpot){
        _preHidingPos = _playerBody.position;
        _playerBody.position = HidingSpot.transform.position;
    }

    public void HideAt(Vector3 HidingLocation){
        _preHidingPos = _playerBody.position;
        _playerBody.position = HidingLocation;
    }

    public void StopHiding(){
        _playerBody.position = _preHidingPos;
    }
	
	private void FixedUpdate () {
        if (_playerStats.GameOver) return;
        GetKeyboardInput();
        GetControllerInput();

    }

    private void GetKeyboardInput()
    {
        if (!_canMove) return;

        if (Input.GetKey(forwardKey)){
            _playerBody.AddRelativeForce(Vector3.forward, ForceMode.VelocityChange);
        }

        if (Input.GetKey(backwardKey)){
            _playerBody.AddRelativeForce(Vector3.back, ForceMode.VelocityChange);
        }

        if (Input.GetKey(moveLeftKey)){
            _playerBody.AddRelativeForce(Vector3.left, ForceMode.VelocityChange);
        }

        if (Input.GetKey(moveRightKey)){
            _playerBody.AddRelativeForce(Vector3.right, ForceMode.VelocityChange);
        }

        if (Input.GetKeyDown(crouchKey)){
            _playerBody.position = new Vector3(_playerBody.position.x, -0.1f, _playerBody.position.z);
        }

        if (Input.GetKeyUp(crouchKey)){
            _playerBody.position = new Vector3(_playerBody.position.x, 1, _playerBody.position.z);
        }
    }

    private void GetControllerInput()
    {
        float rightStickX = Input.GetAxis("C" + controllerNumber + "RSX");
        float rightStickY = Input.GetAxis("C" + controllerNumber + "RSY");
        gameObject.transform.Rotate(new Vector3(0, 1, 0), rightStickX * 2);

        if (!_canMove) return;

        float leftStickX = Input.GetAxis("C" + controllerNumber + "LSX");
        float leftStickY = Input.GetAxis("C" + controllerNumber + "LSY");
        _playerBody.AddRelativeForce(new Vector3(leftStickX, 0, leftStickY), ForceMode.VelocityChange);
        
        
    }
}
