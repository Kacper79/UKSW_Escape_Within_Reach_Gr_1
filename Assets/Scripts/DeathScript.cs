using Assets.Scripts.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class DeathScript : MonoBehaviour
    {

        public void TeleportToCell()
        {
            transform.localPosition = cellTransform.position;
            Physics.SyncTransforms();
        }

        public void TeleportToInfirmary()
        {
            gameObject.transform.position = infirmaryTransform.position;
            Physics.SyncTransforms();
        }

        public void TeleportToSolitary()
        {
            gameObject.transform.position = solitaryTransform.position;
            Physics.SyncTransforms();
        }

        public Transform cellTransform;
        public Transform infirmaryTransform;
        public Transform solitaryTransform;

        public bool walksInRestrictedZone;
        public string restrictedZoneName;
        public bool wantToLeaveZone;
        public bool isBeingEscorted;
        public List<GuardNPC> guardsChasing;
    }
}