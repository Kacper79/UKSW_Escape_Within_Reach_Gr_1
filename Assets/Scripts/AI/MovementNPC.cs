
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class MovementNPC : MonoBehaviour
{
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        ChangeToNextPoint();
    }

    void Update()
    {
        navMeshAgent.destination = currentWalkToPosition;
        if (navMeshAgent.velocity.magnitude == 0.0f && Vector3.Distance(transform.position, currentWalkToPosition) < distanceAtRestEpsilon && !pathReachedLock) //transform.position == currentWalkToPosition
        {
            pathReachedLock = true;
            Debug.Log($"Path completed, Dist: {Vector3.Distance(transform.position, currentWalkToPosition)}");
            currentReachCallback?.Invoke();
            StartCoroutine(WaitForTime());
        }
    }

    public IEnumerator WaitForTime()
    {
        Debug.Log($"Waiting for {currentStayTime} seconds");
        yield return new WaitForSecondsRealtime(currentStayTime);
        Debug.Log("Stopped Waiting");
        ChangeToNextPoint();
        pathReachedLock = false;
    }

    public void ChangeToNextPoint()
    {
        if (currentNavPoint >= navPoints.Count) return;     //No more points to travel
        
        NavigationWaypoint navPoint = navPoints[currentNavPoint];
        currentWalkToPosition = navPoint.destinationPoint.position;
        currentStayTime = navPoint.pointStayTime;
        currentReachCallback = navPoint.onPointReachedCallback;

        currentNavPoint = (currentNavPoint + 1) % navPoints.Count;
    }

    private Vector3 currentWalkToPosition;
    private float currentStayTime;
    private UnityEvent? currentReachCallback;
    private bool pathReachedLock = false;
    private float distanceAtRestEpsilon = 1.0f;
    private NavMeshAgent navMeshAgent;

    private int currentNavPoint = 0;
    public List<NavigationWaypoint> navPoints;

}

[Serializable]
public struct NavigationWaypoint
{
    public Transform destinationPoint;
    public float pointStayTime;
    public UnityEvent? onPointReachedCallback;
}