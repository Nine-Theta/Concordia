using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHandler : MonoBehaviour {

    public Vector3 StartPosition1;
    public Vector3 StartPosition2;

    public GameObject Car;

    private float _spawnCooldown;

	private void Start () {
		
	}
	
	private void FixedUpdate () {

        if (_spawnCooldown <= 0)
        {
            _spawnCooldown = Random.Range(0.1f, 4.0f);
            GameObject car = Instantiate(Car);
            CarMovement carControl = car.GetComponent<CarMovement>();

            carControl.CarSpeed = Random.Range(0.05f, 0.25f);

            if (Random.Range(0, 2) == 0){
                carControl.StartPos = gameObject.transform.position + StartPosition1;
                carControl.EndPos = gameObject.transform.position + StartPosition2;
            }
            else{
                carControl.StartPos = gameObject.transform.position + StartPosition2;
                carControl.EndPos = gameObject.transform.position + StartPosition1;
            }
        }
        else{
            _spawnCooldown -= Time.deltaTime;
        }
        print(_spawnCooldown);
	}
}
