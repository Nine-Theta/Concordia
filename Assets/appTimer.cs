using UnityEngine;

using System.Collections;


// create a script called appTimer ( note the spelling ) and drag it on the terrain


public class appTimer : MonoBehaviour {

 // Use this for initialization

void Start () {


StartCoroutine ("closeApp");  

}  


// Update is called once per frame 

void Update () {
 }


 IEnumerator closeApp() {


yield return new WaitForSeconds(7); 

Application.Quit ();

 }

}

