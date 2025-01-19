using Assets.Scripts.PlayerRelated;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.AI
{
    /// <summary>
    /// This script is being used to kill player that enters unannounced/uninvited in another player cell
    /// </summary>
    public class CellBlockIntrusion : MonoBehaviour
    {

        void OnTriggerEnter(Collider other)
        {
            if (!cellOwnership.Contains(other.gameObject))
            {
                isCellBeingRaided = true;
                offenderGO = other.gameObject;
            } else
            {
                Debug.Log("Cell is being guarded");
                isCellOwnerGuarding = true;
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<PlayerStress>().isResidingInCell = true;
                }
            }
        }

        void OnTriggerStay(Collider other)
        {
            if(isCellBeingRaided && isCellOwnerGuarding && offenderGO != null && !offenderGO.CompareTag("Guard"))
            {
                Debug.Log("The player has entered a guarded cell and has been hurt as a punishment");
                offenderGO.GetComponent<DeathScript>().TeleportToInfirmary();       //Will be hurt from invading another prisoner cell
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (!cellOwnership.Contains(other.gameObject))
            {
                isCellBeingRaided = false;
            } else
            {
                isCellOwnerGuarding = false;
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<PlayerStress>().isResidingInCell = false;
                }
                offenderGO = null;
            }
        }

        public List<GameObject> cellOwnership;
        private GameObject offenderGO;
        public bool isCellBeingRaided;
        public bool isCellOwnerGuarding;
    }
}