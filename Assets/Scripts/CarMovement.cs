using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour {
    
    public Vector3 StartPos;
    public Vector3 EndPos;
    
    public float CarSpeed = 0.1f;

    private void Start () {
        gameObject.transform.position = StartPos;

    }
	
	private void FixedUpdate () {

        if (EndPos == Vector3.zero) return;

        if ((EndPos - gameObject.transform.position).magnitude <= 0.5f) Destroy(this.gameObject);

        gameObject.transform.position += Vector3.Scale(Vector3.Normalize(EndPos - StartPos), new Vector3(CarSpeed, CarSpeed, CarSpeed));

        //print("Endpos: " + EndPos + " pos: " + gameObject.transform.position + " length: " + (EndPos - gameObject.transform.position).magnitude);
    }
}
