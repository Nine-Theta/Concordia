using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartandExitButton : MonoBehaviour
{

    public void Play()
    {
        Application.LoadLevel(1);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
