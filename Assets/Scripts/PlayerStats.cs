using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Scrollbar lifebar;
    public Text GameOverMessage;

    #region PlayerPartsTest
    /**/
    public GameObject Head;
    public GameObject Body;
    public GameObject ArmLeft;
    public GameObject ArmRight;
    /**/
    #endregion

    //public string[] Inventory = new string[20]; //Does not include a pokernight
    public string[] NoteData = new string[20];

    [SerializeField] //Serialize Field shows item in the inspector even if it's private. Shouldn't be accessed by other players but should be in inspector
    private float _playerHealth = 100; //When this drops below 0, the player is considered medically dead.
    private float _maxHealth;

    private bool _gameOver = false;

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
                _playerHealth = 0;
                lifebar.GetComponentInChildren<Image>().gameObject.SetActive(false);
                _gameOver = true;
                GameOverMessage.text = "GAME OVER";
                //Game Over man, Game Over.
                #region PlayerPartsTest
                /**/
                Head.AddComponent<MeshCollider>().convex = true;
                Body.AddComponent<MeshCollider>().convex = true;
                ArmLeft.AddComponent<MeshCollider>().convex = true;
                ArmRight.AddComponent<MeshCollider>().convex = true;
                Head.AddComponent<Rigidbody>().AddExplosionForce(300, gameObject.transform.position, 1000);
                Rigidbody bodyRigid = Body.AddComponent<Rigidbody>();//.AddExplosionForce(1, gameObject.transform.position, 1);
                bodyRigid.constraints = RigidbodyConstraints.FreezePositionZ;
                ArmLeft.AddComponent<Rigidbody>().AddExplosionForce(300, gameObject.transform.position, 1000);
                ArmRight.AddComponent<Rigidbody>().AddExplosionForce(300, gameObject.transform.position, 1000);
                /**/
                #endregion
            }
        }
    }

    public bool GameOver
    {
        get { return _gameOver; }
    }
}
