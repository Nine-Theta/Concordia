using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Rigidbody PlayerBody;
    public KeyCode ForwardKey;
    public KeyCode BackwardKey;
    public KeyCode MoveLeftKey;
    public KeyCode MoveRightKey;
    public KeyCode Crouch;
    //Should be either 1 or 2. 
    public string controllerNumber = "1";
    //public KeyCode RotateLeftKey;
    //public KeyCode RotateRightKey;

    private TriggerHandler _triggerHandler;

    private Vector3 _preHidingPos;

    private void Start () {
        _triggerHandler = this.gameObject.GetComponent<TriggerHandler>();
	}

    //private void OnTriggerEnter(Collider other)
    //{

    //}

    public void HideAt(GameObject HidingSpot){
        _preHidingPos = PlayerBody.position;
        PlayerBody.position = HidingSpot.transform.position;
    }

    public void HideAt(Vector3 HidingLocation){
        _preHidingPos = PlayerBody.position;
        PlayerBody.position = HidingLocation;
    }

    public void StopHiding(){
        PlayerBody.position = _preHidingPos;
    }
	
	private void FixedUpdate () {
        GetKeyboardInput();
        GetControllerInput();

    }

    private void GetKeyboardInput()
    {
        //if(_triggerHandler.I)

        if (Input.GetKey(ForwardKey))
        {
            PlayerBody.AddRelativeForce(Vector3.forward, ForceMode.VelocityChange);
        }

        if (Input.GetKey(BackwardKey))
        {
            PlayerBody.AddRelativeForce(Vector3.back, ForceMode.VelocityChange);
        }

        if (Input.GetKey(MoveLeftKey))
        {
            PlayerBody.AddRelativeForce(Vector3.left, ForceMode.VelocityChange);
        }

        if (Input.GetKey(MoveRightKey))
        {
            PlayerBody.AddRelativeForce(Vector3.right, ForceMode.VelocityChange);
        }

        if (Input.GetKeyDown(Crouch))
        {
            PlayerBody.position = new Vector3(PlayerBody.position.x, -0.1f, PlayerBody.position.z);
        }

        if (Input.GetKeyUp(Crouch))
        {
            PlayerBody.position = new Vector3(PlayerBody.position.x, 1, PlayerBody.position.z);
        }
    }

    private void GetControllerInput()
    {
        float leftStickX = Input.GetAxis("C" + controllerNumber + "LSX");
        float leftStickY = Input.GetAxis("C" + controllerNumber + "LSY");
        float rightStickX = Input.GetAxis("C" + controllerNumber + "RSX");
        float rightStickY = Input.GetAxis("C" + controllerNumber + "RSY");
        PlayerBody.AddRelativeForce(new Vector3(leftStickX, 0, leftStickY), ForceMode.VelocityChange);
        gameObject.transform.Rotate(new Vector3(0, 1, 0), rightStickX);
        
    }
}
