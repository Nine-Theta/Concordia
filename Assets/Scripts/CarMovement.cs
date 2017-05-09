using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{

    public Vector3 startPos;
    public Vector3 endPos;

    public float carSpeed = 0.1f;

    private void Start()
    {
        gameObject.transform.position = startPos;
    }

    private void FixedUpdate()
    {
        DriveBy();
    }

    private void DriveBy()
    {
        if (endPos == Vector3.zero)
            return;
        if ((endPos - gameObject.transform.position).magnitude <= 0.5f)
            Destroy(this.gameObject);
        gameObject.transform.position += Vector3.Scale(Vector3.Normalize(endPos - startPos), new Vector3(carSpeed, carSpeed, carSpeed));
    }
}
