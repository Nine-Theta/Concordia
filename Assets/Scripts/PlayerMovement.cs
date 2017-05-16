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
    public KeyCode pauseKey;
    public KeyCode jumpKey;

    public GameObject findOtherPlayer;
    public Transform otherPlayer;
    public Canvas PauseScreen;

    public string controllerNumber = "1";  //Should be either 1 or 2.

    public float movementSpeed = 1.0f;

    private Rigidbody _playerBody;
    private PlayerStats _playerStats;
    private Animator _animator;

    private Vector3 _preHidingPos;

    private bool _canMove = true;
    private bool _releasedConstraints = false;

    private void Start()
    {
        _playerBody = this.gameObject.GetComponent<Rigidbody>();
        _playerStats = this.gameObject.GetComponent<PlayerStats>();
        _animator = this.gameObject.GetComponent<Animator>();
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

    public void Reset()
    {
        _playerBody.useGravity = true;
        _releasedConstraints = false;
        _canMove = true;
    }

    private void GetKeyboardInput()
    {
        if (Input.GetKeyDown(pauseKey) || Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            PauseScreen.gameObject.SetActive(true);
            PauseScreen.GetComponent<PauseMenu>().PauseKey = pauseKey;
            return;
        }
        _animator.SetBool("IsWalking", false);
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
            _playerBody.AddRelativeForce(Vector3.forward * movementSpeed, ForceMode.VelocityChange);
            _animator.SetBool("IsWalking", true);
        }
        if (Input.GetKey(backwardKey))
        {
            _playerBody.AddRelativeForce(Vector3.back * movementSpeed, ForceMode.VelocityChange);
            _animator.SetBool("IsWalking", true);
        }
        if (Input.GetKey(moveLeftKey))
        {
            _playerBody.AddRelativeForce(Vector3.left * movementSpeed, ForceMode.VelocityChange);
            _animator.SetBool("IsWalking", true);
        }
        if (Input.GetKey(moveRightKey))
        {
            _playerBody.AddRelativeForce(Vector3.right * movementSpeed, ForceMode.VelocityChange);
            _animator.SetBool("IsWalking", true);
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
        if (Input.GetKeyDown(jumpKey))
        {
            _playerBody.AddRelativeForce(Vector3.up * movementSpeed * 10, ForceMode.VelocityChange);
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
        float leftStickX = Input.GetAxis("C" + controllerNumber + "LSX") * movementSpeed;
        float leftStickY = Input.GetAxis("C" + controllerNumber + "LSY") * movementSpeed;
        _playerBody.AddRelativeForce(new Vector3(leftStickX, 0, leftStickY), ForceMode.VelocityChange);
        if(leftStickX != 0.0f || leftStickY != 0.0f)
            _animator.SetBool("IsWalking", true);
    }
}
