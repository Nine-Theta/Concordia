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
    //public KeyCode RotateLeftKey;
    //public KeyCode RotateRightKey;

    private Vector3 _preHidingPos;

    private void Start () {
	}

    //private void OnTriggerEnter(Collider other)
    //{

    //}

    public void HideAt(GameObject HidingSpot){
        _preHidingPos = PlayerBody.position;
        PlayerBody.position = HidingSpot.transform.position;
    }

    public void StopHiding(){
        PlayerBody.position = _preHidingPos;
    }
	
	private void FixedUpdate () {
        if (Input.GetKey(ForwardKey)){
            PlayerBody.AddRelativeForce(Vector3.forward, ForceMode.VelocityChange);           
        }

        if (Input.GetKey(BackwardKey)){
            PlayerBody.AddRelativeForce(Vector3.back, ForceMode.VelocityChange);
        }

        if (Input.GetKey(MoveLeftKey)){
            PlayerBody.AddRelativeForce(Vector3.left, ForceMode.VelocityChange);
        }

        if (Input.GetKey(MoveRightKey)){
            PlayerBody.AddRelativeForce(Vector3.right, ForceMode.VelocityChange);
        }

        if (Input.GetKeyDown(Crouch)){
            PlayerBody.position = new Vector3(PlayerBody.position.x, -0.1f, PlayerBody.position.z);
        }

        if (Input.GetKeyUp(Crouch)){
            PlayerBody.position = new Vector3(PlayerBody.position.x, 1, PlayerBody.position.z);
        }

        //if (Input.GetKey(RotateLeftKey)){
        //    PlayerBody.AddRelativeForce(Vector3.forward, ForceMode.Acceleration);
        //}

        //if (Input.GetKey(RotateRightKey)){
        //    PlayerBody.AddRelativeForce(Vector3.forward, ForceMode.Acceleration);
        //}
    }
}
