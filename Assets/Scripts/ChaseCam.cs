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
        Chase();
	}

    void Chase()
    {
        RaycastHit hit;
        cam.transform.localPosition = idealPos;
        Ray ray = new Ray(gameObject.transform.position, new Vector3(cam.transform.position.x - gameObject.transform.position.x, cam.transform.position.y - gameObject.transform.position.y, cam.transform.position.z - gameObject.transform.position.z).normalized);
        if (Physics.Raycast(ray, out hit, idealPos.magnitude))
        {
            cam.transform.position = hit.point;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, idealPos.y, cam.transform.localPosition.z);
        }
    }
}
