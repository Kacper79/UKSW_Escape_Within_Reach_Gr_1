using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This class is used to simulate coin throwing physics.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SimulateCoin : MonoBehaviour
{
    void OnEnable()
    {
        coinRigidbody = GetComponent<Rigidbody>();
        coinRigidbody.useGravity = false;
    }

    /// <summary>
    /// Use this function to start calculating coin throwing physics
    /// </summary>
    /// <param name="throwAngle"></param>
    public void StartCoinThrow(float throwAngle)
    {
        isFlying = true;
        coinRigidbody.useGravity = true;
        this.throwAngle = throwAngle;
        //Debug.Break();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Podloga")) 
        {
            isFlying = false;
            coinRigidbody.isKinematic = true;
            Debug.Log("The coin has landed");
            Collider[] nearbyNPCs = Physics.OverlapSphere(transform.position, detectRadius);
            if(nearbyNPCs.Length > 0)
            {
                for(int i = 0; i < nearbyNPCs.Length; i++)
                {
                    if (nearbyNPCs[i].gameObject.TryGetComponent(out MovementNPC mnc) && !mnc.patrolOverride && mnc.isDistractable)
                    {
                        mnc.OverrideNextPoint(transform.position, coinPickupTime);
                        break;
                    }
                }
            }
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (isFlying)
        {
            coinRigidbody.AddForce(transform.forward*throwAngle, ForceMode.VelocityChange);
            //coinRigidbody.AddForce(-Vector3.up * 0.23f, ForceMode.VelocityChange);
            //if(throwAngle - 0.01f <= 0) throwAngle -= 0.01f;
        }
    }

    /*void OnDrawGizmos()
    {
        if(isFlying) Gizmos.DrawWireSphere(transform.position, detectRadius);
    }*/

    Rigidbody coinRigidbody;
    float throwAngle;
    float coinPickupTime = 2.0f;
    float detectRadius = 5.0f;
    bool isFlying = false;
}
