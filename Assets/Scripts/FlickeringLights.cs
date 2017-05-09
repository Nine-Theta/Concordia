using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLights : MonoBehaviour {
    public bool paused = false;
    public FlickerType flickerType = FlickerType.RandomizedFlicker;
    public float randMin = 0.1f;
    public float randMax = 1.1f;
    /// <summary>
    /// The durations it takes in between toggling (cycles through after the last one ended)
    /// </summary>
    public float[] timerCycle;
    

    private float _flickerTimer;
    private Light _thisLight;
    private CapsuleCollider _thisBody;
    private int _currentTimer;

    // Use this for initialization
    private void Start () {
        _thisLight = gameObject.GetComponent<Light>();
        _thisBody = gameObject.GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate () {
        Flicker();
        
	}

    private void Flicker()
    {
        if (!paused)
        {
            switch (flickerType)
            {
                case FlickerType.RandomizedFlicker:
                    _flickerTimer -= Time.deltaTime;
                    if (_flickerTimer <= 0.0f)
                    {
                        _thisLight.enabled = !_thisLight.enabled;
                        _thisBody.enabled = !_thisBody.enabled;
                        _flickerTimer = Random.Range(randMin, randMax);
                    }
                    break;
                case FlickerType.ControlledTimerIntervals:
                    _flickerTimer -= Time.deltaTime;
                    if (_flickerTimer <= 0.0f)
                    {
                        _thisLight.enabled = !_thisLight.enabled;
                        _thisBody.enabled = !_thisBody.enabled;
                        _currentTimer++;
                        if (_currentTimer >= timerCycle.Length)
                            _currentTimer = 0;
                        _flickerTimer = timerCycle[_currentTimer];
                    }
                    break;
            }
        }
    }

    public void TogglePause()
    {
        paused = !paused;
    }

    public void TurnOff()
    {
        _thisLight.enabled = false;
        _thisBody.enabled = false;
    }
}

public enum FlickerType
{
    RandomizedFlicker = 0,
    ControlledTimerIntervals,

}