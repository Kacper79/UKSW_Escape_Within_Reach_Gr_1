using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTimeEventEnter : MonoBehaviour
{
    [SerializeField] private QTEManager qte_manager;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            qte_manager.StartQTE("Naci�nij Q, by unikn�� agresywnego wi�nia!");
        }
    }
}
