using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public KeyCode forwardKey;
    public KeyCode backwardKey;
    public KeyCode moveLeftKey;
    public KeyCode moveRightKey;
    public KeyCode rotateRightKey;
    public KeyCode rotateLeftKey;
    //public KeyCode crouchKey;
    public KeyCode findOtherKey;

    public string controllerNumber = "1";  //Should be either 1 or 2.

    public GameObject findOtherPlayer;
    public Transform otherPlayer;

    private Rigidbody _playerBody;
    private PlayerStats _playerStats;

    private Vector3 _preHidingPos;

    private bool _canMove = true;
    private bool _releasedConstraints = false;

    private void Start()
    {
        _playerBody = this.gameObject.GetComponent<Rigidbody>();
        _playerStats = this.gameObject.GetComponent<PlayerStats>();
    }

    public bool canMove
    {
        get { return _canMove; }
        set { _canMove = value; }
    }

    public void HideAt(Vector3 HidingLocation)
    {
        _playerBody.velocity = new Vector3(0, 0, 0);
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        _preHidingPos = _playerBody.position;
        _playerBody.position = HidingLocation;
    }

    public void StopHiding()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        _playerBody.position = _preHidingPos;
    }

    private void FixedUpdate()
    {
        if (_releasedConstraints) return;
        CheckGameOver();
        GetKeyboardInput();
        GetControllerInput();
    }

    private void CheckGameOver()
    {
        if (_playerStats.GameOver)
        {
            //_playerBody.freezeRotation = false;
            _playerBody.useGravity = true;
            //_playerBody.AddRelativeTorque(0.9f, 0.75f, 0.9f, ForceMode.Impulse);
            _releasedConstraints = true;
        }
    }

    private void GetKeyboardInput()
    {
        #region RotationForTesting
        if (Input.GetKey(rotateLeftKey))
        {
            gameObject.transform.Rotate(new Vector3(0, 1, 0), -1);
        }
        if (Input.GetKey(rotateRightKey))
        {
            gameObject.transform.Rotate(new Vector3(0, 1, 0), 1);
        }
        #endregion
        if (!_canMove)
            return;
        #region KeysAndForce
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
        #endregion
        //if (Input.GetKeyDown(crouchKey))
        //{
        //    _playerBody.position = new Vector3(_playerBody.position.x, -0.1f, _playerBody.position.z);
        //}
        if (Input.GetKeyDown(findOtherKey))
        {
            GameObject prefab = Instantiate(findOtherPlayer, transform.position, transform.rotation);
            prefab.GetComponent<TrackOtherHalf>().SetTarget(otherPlayer.transform);
        }
        //if (Input.GetKeyUp(crouchKey))
        //{
        //    _playerBody.position = new Vector3(_playerBody.position.x, 1, _playerBody.position.z);
        //}
    }

    private void GetControllerInput()
    {
        float rightStickX = Input.GetAxis("C" + controllerNumber + "RSX");
        //This one is set up properly, but not necessary so to surpress the warning it's commented out
        //float rightStickY = Input.GetAxis("C" + controllerNumber + "RSY");
        gameObject.transform.Rotate(new Vector3(0, 1, 0), rightStickX * 2);

        if (!_canMove)
            return;
        float leftStickX = Input.GetAxis("C" + controllerNumber + "LSX");
        float leftStickY = Input.GetAxis("C" + controllerNumber + "LSY");
        _playerBody.AddRelativeForce(new Vector3(leftStickX, 0, leftStickY), ForceMode.VelocityChange);
    }
}
