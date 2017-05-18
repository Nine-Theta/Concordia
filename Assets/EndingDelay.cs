using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingDelay : MonoBehaviour
{
    public float delayInSeconds = 1.0f;

    private void FixedUpdate()
    {
        delayInSeconds -= Time.deltaTime;
        if (delayInSeconds <= 0.0f)
        {
          SceneManager.LoadSceneAsync(0);
          this.enabled = false;
        }
    }
}
