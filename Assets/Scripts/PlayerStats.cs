using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    private bool gameOver = false;
    public Scrollbar lifebar;
    //Serialize Field shows item in the inspector even if it's private
    [SerializeField]
    private float playerHealth = 100; //When this drops below 0, the player is considerd dead.

    private float _maxHealth;
    
    public float MaxHealth
    {
        get { return _maxHealth; }
    }

    /// <summary>
    /// Public getter for the boundary
    /// </summary>
    public float PlayerHealth
    {
        get
        {
            return playerHealth;
        }
        set
        {
            if(value > 0)
            {
                playerHealth = value;
                lifebar.size = playerHealth / _maxHealth;
            }
            else if(!gameOver)
            {
                playerHealth = 0;
                lifebar.GetComponentInChildren<Image>().gameObject.SetActive(false);
                gameOver = true;
                //Game Over
            }
        }
    }
	private void Start () {
        _maxHealth = playerHealth;
	}
	
	private void Update () {
		
	}
}
