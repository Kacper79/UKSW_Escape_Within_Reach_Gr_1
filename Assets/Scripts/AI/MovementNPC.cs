
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
        npcAnimator = GetComponent<Animator>();

        npcAnimator.SetBool("isWalking", false);
        npcAnimator.SetBool("isCrouching", false);
        npcAnimator.SetBool("isRunning", false);
        ChangeToNextPoint();
        npcAnimator.SetBool("isWalking", true);
    }

    void Update()
    {
        if (pathReachedLock) return;

        if (patrolOverride && !isOverrideVector) navMeshAgent.destination = currentOverrideTransform.position;
        else navMeshAgent.destination = currentWalkToPosition;

        float dist = Vector3.Distance(transform.position, currentWalkToPosition);
        //Debug.Log("Dist: " + dist);
        if (navMeshAgent.velocity.magnitude < 0.1f && dist < distanceAtRestEpsilon && !pathReachedLock) //transform.position == currentWalkToPosition
        {
            pathReachedLock = true;
            //Debug.Log($"Path completed, Dist: {Vector3.Distance(transform.position, currentWalkToPosition)}");
            currentReachCallback?.Invoke();
            StartCoroutine(WaitForTime());
        }
    }

    public IEnumerator WaitForTime()
    {
        //Debug.Log($"Waiting for {currentStayTime} seconds");
        npcAnimator.SetBool("isWalking",false);
        yield return new WaitForSecondsRealtime(currentStayTime);
        npcAnimator.SetBool("isWalking", true);
        ChangeToNextPoint();
        patrolOverride = false;
        pathReachedLock = false;
        isOverrideVector = false;
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

    public void OverrideNextPoint(Transform dstPoint, float waitTime)
    {
        if (pathReachedLock) return;
        patrolOverride = true;
        currentOverrideTransform = dstPoint;
        currentStayTime = waitTime;
    }

    public void OverrideNextPoint(Vector3 dstPoint, float waitTime)
    {
        if (pathReachedLock) return;
        patrolOverride = true;
        currentWalkToPosition = dstPoint;
        currentStayTime = waitTime;
        isOverrideVector = true;
    }

    public void FlushOverride()
    {
        pathReachedLock = true;
        isOverrideVector = false;
        StartCoroutine(WaitForTime());
    }

    private Vector3 currentWalkToPosition;
    private float currentStayTime;
    private Transform currentOverrideTransform;
    public bool patrolOverride;
    private bool isOverrideVector = false;
    private UnityEvent? currentReachCallback;
    private bool pathReachedLock = false;
    private float distanceAtRestEpsilon = 1.5f;
    private NavMeshAgent navMeshAgent;

    private int currentNavPoint = 0;
    public List<NavigationWaypoint> navPoints;
    private Animator npcAnimator;

}

[Serializable]
public struct NavigationWaypoint
{
    public Transform destinationPoint;
    public float pointStayTime;
    public UnityEvent? onPointReachedCallback;
}