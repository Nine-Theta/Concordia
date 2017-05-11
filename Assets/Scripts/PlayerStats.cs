using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Scrollbar lifebar;
    public Text GameOverMessage;

    #region PlayerPartsTest
    private GameObject _head;
    private GameObject _body;
    private GameObject _armLeft;
    private GameObject _armRight;
    #endregion


    public GameObject bodyPrefab;
    public string[] NoteData = new string[20];

    [SerializeField] //Serialize Field shows item in the inspector even if it's private. Shouldn't be accessed by other players but should be in inspector
    private float _playerHealth = 100; //When this drops below 0, the player is considered medically dead.
    private float _maxHealth;

    private bool _gameOver = false;

    private void Start()
    {
        AssignBodyParts();
    }

    private void Awake()
    {
        _maxHealth = _playerHealth;
    }

    public float MaxHealth
    {
        get { return _maxHealth; }
    }

    /// <summary>
    /// Public getter for the boundary
    /// </summary>
    public float PlayerHealth
    {
        get { return _playerHealth; }

        set
        {
            if (value > 0)
            {
                _playerHealth = value;
                lifebar.size = _playerHealth / _maxHealth;
            }
            else if (!_gameOver)
            {
                Die();
            }
        }
    }

    public bool GameOver
    {
        get { return _gameOver; }
    }

    public void Respawn(Vector3 position)
    {
        gameObject.transform.position = position;
        _gameOver = false;
        _playerHealth = _maxHealth;
        lifebar.GetComponentInChildren<Image>().gameObject.SetActive(true);
        lifebar.size = 1;
        GameOverMessage.text = "";
        #region newDestruction
        foreach(Transform child in gameObject.GetComponentInChildren<Transform>())
        {
            if(child.CompareTag("Body"))
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        #endregion
        #region oldDestruction
        /**/
        Destroy(_head.GetComponent<MeshCollider>());
        Destroy(_body.GetComponent<MeshCollider>());
        Destroy(_armLeft.GetComponent<MeshCollider>());
        Destroy(_armRight.GetComponent<MeshCollider>());
        Destroy(_head.GetComponent<Rigidbody>());
        Destroy(_body.GetComponent<Rigidbody>());
        Destroy(_armLeft.GetComponent<Rigidbody>());
        Destroy(_armRight.GetComponent<Rigidbody>());
        /**/
        #endregion
        GameObject newBody = GameObject.Instantiate<GameObject>(bodyPrefab, gameObject.transform);
        newBody.transform.localPosition = new Vector3(0, 0, 0);
        newBody.transform.localEulerAngles = new Vector3(0, 0, 0);
        AssignBodyParts();
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Assigns variables to their respective body parts
    /// </summary>
    private void AssignBodyParts()
    {
        foreach (MeshFilter child in gameObject.GetComponentsInChildren<MeshFilter>())
        {
            string[] splitName = child.gameObject.name.Split('_');
            switch (splitName[1])
            {
                case "Head":
                    _head = child.gameObject;
                    //_headLocalPos = child.transform.localPosition;
                    //Debug.Log(_headLocalPos);
                    break;
                case "Body":
                    _body = child.gameObject;
                    //_bodyLocalPos = child.transform.localPosition;
                    //Debug.Log(_bodyLocalPos);
                    break;
                case "HandL":
                    _armLeft = child.gameObject;
                    //_armLeftLocalPos = child.transform.localPosition;
                    //Debug.Log(_armLeftLocalPos);
                    break;
                case "HandR":
                    _armRight = child.gameObject;
                    //_armRightLocalPos = child.transform.localPosition;
                    //Debug.Log(_armRightLocalPos);
                    break;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Die()
    {
        _playerHealth = 0;
        lifebar.GetComponentInChildren<Image>().gameObject.SetActive(false);
        _gameOver = true;
        GameOverMessage.text = "GAME OVER";
        //Game Over man, Game Over.

        #region PlayerPartsTest
        _head.AddComponent<MeshCollider>().convex = true;
        _body.AddComponent<MeshCollider>().convex = true;
        _armLeft.AddComponent<MeshCollider>().convex = true;
        _armRight.AddComponent<MeshCollider>().convex = true;
        _head.AddComponent<Rigidbody>().AddExplosionForce(300, gameObject.transform.position, 0);
        Rigidbody bodyRigid = _body.AddComponent<Rigidbody>();//.AddExplosionForce(1, gameObject.transform.position, 1);
        bodyRigid.constraints = RigidbodyConstraints.FreezePositionZ;
        _armLeft.AddComponent<Rigidbody>().AddExplosionForce(300, gameObject.transform.position, 1000);
        _armRight.AddComponent<Rigidbody>().AddExplosionForce(300, gameObject.transform.position, 1000);
        #endregion
    }
}
