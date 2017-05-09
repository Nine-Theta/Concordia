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
