using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public Scrollbar lifebar;
    public Text GameOverMessage;
    
    [SerializeField] //Serialize Field shows item in the inspector even if it's private
    private float _playerHealth = 100; //When this drops below 0, the player is considered medically dead.
    private float _maxHealth;

    private bool _gameOver = false;

    private void Awake(){
        _maxHealth = _playerHealth;
    }

    public float MaxHealth{
        get { return _maxHealth; }
    }

    /// <summary>
    /// Public getter for the boundary
    /// </summary>
    public float PlayerHealth{
        get { return _playerHealth; }

        set{
            if(value > 0){
                _playerHealth = value;
                lifebar.size = _playerHealth / _maxHealth;
            }
            else if(!_gameOver){
                _playerHealth = 0;
                lifebar.GetComponentInChildren<Image>().gameObject.SetActive(false);
                _gameOver = true;
                GameOverMessage.text = "GAME OVER";
                //Game Over man, Game Over.
            }
        }
    }

    public bool GameOver{
        get { return _gameOver; }
    }
}
