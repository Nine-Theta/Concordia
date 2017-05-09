using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHandler : MonoBehaviour
{

    public Vector3 startPosition1;
    public Vector3 startPosition2;

    public GameObject Car;

    private float _spawnCooldown;

    private void Start()
    {

    }

    private void FixedUpdate()
    {

        if (_spawnCooldown <= 0)
        {
            _spawnCooldown = Random.Range(0.1f, 4.0f);
            GameObject car = Instantiate(Car);
            CarMovement carControl = car.GetComponent<CarMovement>();

            carControl.carSpeed = Random.Range(0.05f, 0.25f);

            if (Random.Range(0, 2) == 0)
            {
                carControl.startPos = gameObject.transform.position + startPosition1;
                carControl.endPos = gameObject.transform.position + startPosition2;
            }
            else
            {
                carControl.startPos = gameObject.transform.position + startPosition2;
                carControl.endPos = gameObject.transform.position + startPosition1;
            }
        }
        else
        {
            _spawnCooldown -= Time.deltaTime;
        }
    }
}
