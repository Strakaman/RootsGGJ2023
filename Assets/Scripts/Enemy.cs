using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public Transform target;
    NavMeshAgent agent;
    Vector3 runPoint;
    bool runPointTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!runPointTriggered)
        {
            TriggerRunPoint();
        }

        if (runPointTriggered)
        {
            agent.destination = runPoint;
        }
        
        if (DistanceBetween(transform.position, runPoint) <= 0.1)
        {
            runPointTriggered = false;
        }
    }

    private void TriggerRunPoint()
    {
        runPoint = target.position;
        runPointTriggered = true;
    }

    private float DistanceBetween(Vector3 pointA, Vector3 pointB)
    {
        float totSqr = 0;
        totSqr += Mathf.Pow(pointB.x - pointA.x, 2);
        totSqr += Mathf.Pow(pointB.y - pointA.y, 2);
        totSqr += Mathf.Pow(pointB.z - pointA.z, 2);
        return Mathf.Sqrt(totSqr);
    }
}
