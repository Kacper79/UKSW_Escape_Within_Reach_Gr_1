using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimulateCoin : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        coinRigidbody = GetComponent<Rigidbody>();
        coinRigidbody.useGravity = false;
    }

    public void StartCoinThrow(float throwAngle)
    {
        isFlying = true;
        coinRigidbody.useGravity = true;
        this.throwAngle = throwAngle;
        Debug.Break();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Podloga")) 
        {
            isFlying = false;
            coinRigidbody.isKinematic = true;
            Debug.Log("The coin has landed");
            Collider[] nearbyNPCs = Physics.OverlapSphere(transform.position, 5.0f);
            if(nearbyNPCs.Length > 0)
            {
                for(int i = 0; i < nearbyNPCs.Length; i++)
                {
                    if (nearbyNPCs[i].gameObject.CompareTag("Guard") && nearbyNPCs[i].gameObject.TryGetComponent(out MovementNPC mnc) && !mnc.patrolOverride)
                    {
                        mnc.OverrideNextPoint(transform.position, coinPickupTime);
                        break;
                    }
                }
            }
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*if (Input.GetKey(KeyCode.F8))
        {
            StartCoinThrow();
        }*/
        if (isFlying)
        {
            coinRigidbody.AddForce(transform.forward*throwAngle, ForceMode.VelocityChange);
            //coinRigidbody.AddForce(-Vector3.up * 0.23f, ForceMode.VelocityChange);
            //throwAngle -= Time.deltaTime;
        }
    }

    Rigidbody coinRigidbody;
    float throwAngle;
    float coinPickupTime = 2.0f;
    bool isFlying = false;
}
