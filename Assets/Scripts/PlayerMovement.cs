using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public KeyCode forwardKey;
    public KeyCode backwardKey;
    public KeyCode moveLeftKey;
    public KeyCode moveRightKey;
    public KeyCode crouchKey;
    //Should be either 1 or 2. 
    public string controllerNumber = "1";
    //public KeyCode RotateLeftKey;
    //public KeyCode RotateRightKey;

    private TriggerHandler _triggerHandler;
    private Rigidbody _playerBody;
    private bool _canMove = true;
    private Vector3 _preHidingPos;

    public bool canMove
    {
        get { return _canMove; }
        set { _canMove = value; }
    }

    private void Start () {
        _triggerHandler = this.gameObject.GetComponent<TriggerHandler>();
        _playerBody = this.gameObject.GetComponent<Rigidbody>();
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
        GetKeyboardInput();
        GetControllerInput();

    }

    private void GetKeyboardInput()
    {
        if (!_canMove)
            return;
        if (Input.GetKey(forwardKey))
        {
            _playerBody.AddRelativeForce(Vector3.forward, ForceMode.VelocityChange);
        }

        if (Input.GetKey(backwardKey))
        {
            _playerBody.AddRelativeForce(Vector3.back, ForceMode.VelocityChange);
        }

        if (Input.GetKey(moveLeftKey))
        {
            _playerBody.AddRelativeForce(Vector3.left, ForceMode.VelocityChange);
        }

        if (Input.GetKey(moveRightKey))
        {
            _playerBody.AddRelativeForce(Vector3.right, ForceMode.VelocityChange);
        }

        if (Input.GetKeyDown(crouchKey))
        {
            _playerBody.position = new Vector3(_playerBody.position.x, -0.1f, _playerBody.position.z);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            _playerBody.position = new Vector3(_playerBody.position.x, 1, _playerBody.position.z);
        }
    }

    private void GetControllerInput()
    {
        if (!_canMove)
            return;
        float leftStickX = Input.GetAxis("C" + controllerNumber + "LSX");
        float leftStickY = Input.GetAxis("C" + controllerNumber + "LSY");
        float rightStickX = Input.GetAxis("C" + controllerNumber + "RSX");
        float rightStickY = Input.GetAxis("C" + controllerNumber + "RSY");
        _playerBody.AddRelativeForce(new Vector3(leftStickX, 0, leftStickY), ForceMode.VelocityChange);
        gameObject.transform.Rotate(new Vector3(0, 1, 0), rightStickX);
        
    }
}
