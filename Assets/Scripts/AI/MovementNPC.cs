
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// A script handing the moving the NPC along the path (between patrol points) 
/// </summary>
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

        if (navPoints.Count == 0)
        {
            pathReachedLock = true;
            npcAnimator.SetBool("isWalking", false);
        }
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

    /// <summary>
    /// This coroutine is being called whenever the NPC arrives successfully at each path's patrol point
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Handles changing the next patrol point
    /// </summary>
    public void ChangeToNextPoint()
    {
        if (currentNavPoint >= navPoints.Count) return;     //No more points to travel
        
        NavigationWaypoint navPoint = navPoints[currentNavPoint];
        currentWalkToPosition = navPoint.destinationPoint.position;
        currentStayTime = navPoint.pointStayTime;
        currentReachCallback = navPoint.onPointReachedCallback;

        currentNavPoint = (currentNavPoint + 1) % navPoints.Count;
    }

    /// <summary>
    /// Overrides the patrol path to a given position.
    /// Used in redirecting NPCs when the player throw coins to distract them
    /// </summary>
    /// <param name="dstPoint">transform of the overriden point</param>
    /// <param name="waitTime">how long to wait at arrived override point</param>
    public void OverrideNextPoint(Transform dstPoint, float waitTime)
    {
        if (pathReachedLock) return;
        patrolOverride = true;
        currentOverrideTransform = dstPoint;
        currentStayTime = waitTime;
    }
    /// <summary>
    /// Overrides the patrol path to a given position.
    /// Used in redirecting NPCs when the player throw coins to distract them
    /// </summary>
    /// <param name="dstPoint">positon of the overriden point</param>
    /// <param name="waitTime">how long to wait at arrived override point</param>
    public void OverrideNextPoint(Vector3 dstPoint, float waitTime)
    {
        if (pathReachedLock) return;
        patrolOverride = true;
        currentWalkToPosition = dstPoint;
        currentStayTime = waitTime;
        isOverrideVector = true;
    }

    /// <summary>
    /// Flushing the patrol path's override when NPC successfully arrived at override point
    /// </summary>
    public void FlushOverride()
    {
        pathReachedLock = true;
        isOverrideVector = false;
        StartCoroutine(WaitForTime());
    }

    private Vector3 currentWalkToPosition;
    private float currentStayTime;
    private Transform currentOverrideTransform;
    private bool isOverrideVector = false;
    private UnityEvent? currentReachCallback;
    private bool pathReachedLock = false;
    /// <summary>
    /// Is the patrol path has been overrided
    /// </summary>
    public bool patrolOverride;
    /// <summary>
    /// Describes how much does the position of the guard differes from the patrol point.
    /// </summary>
    private float distanceAtRestEpsilon = 1.5f;
    /// <summary>
    /// Reference to a component used to do pathfinding for the NPC
    /// </summary>
    private NavMeshAgent navMeshAgent;

    /// <summary>
    /// Currently chosen navigation point from patrol path
    /// </summary>
    private int currentNavPoint = 0;
    /// <summary>
    /// Edit this list to add navigation points to patrol path
    /// </summary>
    public List<NavigationWaypoint> navPoints;
    /// <summary>
    /// Reference used to change NPC's animation
    /// </summary>
    private Animator npcAnimator;
    /// <summary>
    /// Is able to be distraced by i.e coin
    /// </summary>
    public bool isDistractable;

}

/// <summary>
/// Stores the destinatio and wait time on the patrol point and (optionally) a callback
/// </summary>
[Serializable]
public struct NavigationWaypoint
{
    public Transform destinationPoint;
    public float pointStayTime;
    public UnityEvent? onPointReachedCallback;
}