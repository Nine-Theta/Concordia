using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrackOtherHalf : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _target;
    // Use this for initialization
    void Start()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _agent.SetDestination(_target.position);
    }

    void FixedUpdate()
    {
        _agent.SetDestination(_target.position);
        if (_agent.remainingDistance <= 1)
        {
            DestroyThis();
        }
    }

    public void SetTarget(Transform pTarget)
    {
        _target = pTarget;
    }

    private void DestroyThis()
    {
        GameObject.Destroy(this.gameObject);
    }
}
