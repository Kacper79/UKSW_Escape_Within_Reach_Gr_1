using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.AI
{
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
                offenderGO = null;
            }
        }

        public List<GameObject> cellOwnership;
        private GameObject offenderGO;
        public bool isCellBeingRaided;
        public bool isCellOwnerGuarding;
    }
}