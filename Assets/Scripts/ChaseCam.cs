using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCam : MonoBehaviour {
    private Vector3 idealPos;
    private GameObject cam;
	// Use this for initialization
	void Start () {
        cam = gameObject.GetComponentInChildren<Camera>().gameObject;
        idealPos = cam.gameObject.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        //Chase();
	}

    void Chase()
    {
        RaycastHit hit;
        Ray ray = new Ray(gameObject.transform.position, new Vector3(idealPos.x, idealPos.y, idealPos.z).normalized);
        if (Physics.Raycast(ray, out hit, idealPos.magnitude))
        {
            cam.transform.position = hit.point;
            //cam.transform.rotation = idealRotation;
            Debug.Log("Raycast hit");
        }
        else
        {
            cam.transform.localPosition = idealPos;
        }
    }
}
