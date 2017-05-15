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
    private GameObject _hpPlane;
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
                if(_hpPlane != null)
                    _hpPlane.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", new Vector2(1 - (_playerHealth / _maxHealth) , - 1 ));
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
        gameObject.GetComponent<Animator>().enabled = false;
        lifebar.size = 1;
        GameOverMessage.text = "";
        #region Destruction
        foreach(Transform child in gameObject.GetComponentInChildren<Transform>())
        {
            if(child.CompareTag("Body"))
            {
                Destroy(child.gameObject);
            }
        }
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
            if (splitName.Length > 1)
            {
                switch (splitName[1])
                {
                    case "Head":
                        _head = child.gameObject;
                        break;
                    case "Body":
                        _body = child.gameObject;
                        break;
                    case "HandL":
                        _armLeft = child.gameObject;
                        break;
                    case "HandR":
                        _armRight = child.gameObject;
                        break;
                }
            }
            if(splitName[0] == "Plane")
            {
                _hpPlane = child.gameObject;
            }
        }
    }

    /// <summary>
    /// u ded lel
    /// </summary>
    private void Die()
    {
        _playerHealth = 0;
        lifebar.GetComponentInChildren<Image>().gameObject.SetActive(false);
        _gameOver = true;
        GameOverMessage.text = "GAME OVER";
        gameObject.GetComponent<Animator>().enabled = false;
        //Game Over man, Game Over.

        #region BestFeature
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
