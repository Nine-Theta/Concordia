using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraDelay : MonoBehaviour
{
    public float delayInSeconds = 1.0f;

    public bool lastCam = false;

    private void FixedUpdate()
    {
        delayInSeconds -= Time.deltaTime;
        if (delayInSeconds <= 0.0f)
        {
            if (lastCam)
                SceneManager.LoadScene(1); //Level

            gameObject.GetComponent<Camera>().enabled = false;
            this.enabled = false;
        }
    }
}
