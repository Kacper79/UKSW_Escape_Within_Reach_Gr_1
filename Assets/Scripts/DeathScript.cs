using Assets.Scripts.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// This class handles teleporting and punishing the player after his death
    /// </summary>
    public class DeathScript : MonoBehaviour
    {
        /// <summary>
        /// Teleport player to his cell and punish him
        /// </summary>
        public void TeleportToCell()
        {
            transform.localPosition = cellTransform.position;
            Physics.SyncTransforms();
            SavePlayerDeathCount();
        }

        /// <summary>
        /// Teleport player to infirmary and punish him
        /// </summary>
        public void TeleportToInfirmary()
        {
            gameObject.transform.position = infirmaryTransform.position;
            Physics.SyncTransforms();
            SavePlayerDeathCount();
        }

        /// <summary>
        /// Teleport player to solitary and punish him
        /// </summary>
        public void TeleportToSolitary()
        {
            gameObject.transform.position = solitaryTransform.position;
            Physics.SyncTransforms();
            SavePlayerDeathCount();
        }

        private void SavePlayerDeathCount()
        {
            if (gameObject.CompareTag("Player"))
            {
                PlayerPrefs.SetInt("deathCount", PlayerPrefs.GetInt("deathCount") + 1);
            }
        }

        /// <summary>
        /// Transform of the player's cell
        /// </summary>
        public Transform cellTransform;
        /// <summary>
        /// Transform of the prison infirmary
        /// </summary>
        public Transform infirmaryTransform;
        /// <summary>
        /// Transform of the prison solitary
        /// </summary>
        public Transform solitaryTransform;
        /// <summary>
        /// Whether walks in the restricted zone
        /// </summary>
        public bool walksInRestrictedZone;
        /// <summary>
        /// Restricted zone name
        /// </summary>
        public string restrictedZoneName;
        /// <summary>
        /// Does prisoner wants to leave restricted zone
        /// </summary>
        public bool wantToLeaveZone;
        /// <summary>
        /// Is being escorted in the restricted zone;
        /// </summary>
        public bool isBeingEscorted;
        /// <summary>
        /// Lists containing all of the guards chasing a prisoner
        /// </summary>
        public List<GuardNPC> guardsChasing;
    }
}