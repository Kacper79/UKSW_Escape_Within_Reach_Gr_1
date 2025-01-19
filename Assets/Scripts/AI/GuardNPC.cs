using Assets.Scripts.PlayerRelated;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI
{
    /// <summary>
    /// This script handles the guard chasing the prisoner that broke the prison rules
    /// </summary>
    public class GuardNPC : MonoBehaviour
    {
        void Update()
        {
            if (isGuardInChase && Vector3.Distance(chasedPrisoner.transform.position, transform.parent.transform.position) < chaseEpsilon)
            {
                //Kill the prisoner
                var dsc = chasedPrisoner.GetComponent<DeathScript>();
                Debug.Log("Killed the prisoner");
                dsc.guardsChasing.ForEach(guard => StartCoroutine(guard.LoseChaseFocus()));
                dsc.guardsChasing.Clear();
                dsc.TeleportToCell();
                //if (transform.parent.gameObject.TryGetComponent(out MovementNPC mnc)) mnc.FlushOverride();
                isGuardInChase = false;
                canApplyStress = false;
                stressLock = false;
            }
            else if (isGuardInChase && canApplyStress && !stressLock)
            {
                Debug.Log("Gaining stress in chase");
                StartCoroutine(GainStressInChase());
            }
        }

        /// <summary>
        /// Call this coroutine to lose the guard's focus and stop chasing
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoseChaseFocus()
        {
            yield return new WaitForSecondsRealtime(lostFocusTime);
            if(chasedPrisoner.TryGetComponent(out DeathScript dsc) && !dsc.walksInRestrictedZone || !isGuardInChase)
            {
                dsc.guardsChasing.Remove(this);
                Debug.Log("Lost chase focus");
                if (transform.parent.gameObject.TryGetComponent(out MovementNPC mnc)) mnc.FlushOverride();
                isGuardInChase = false;
                canApplyStress = false;
                stressLock = false;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (isGuardInChase) return;

            if(other.gameObject.TryGetComponent(out DeathScript dsc) && dsc.walksInRestrictedZone)
            {
                isGuardInChase = true;
                Debug.Log("Guard chasing");
                chasedPrisoner = other.gameObject;
                transform.parent.gameObject.GetComponent<MovementNPC>().OverrideNextPoint(other.gameObject.transform, 1.0f);
                dsc.guardsChasing.Add(this);

                if (chasedPrisoner.CompareTag("Player"))
                {
                    canApplyStress = true;
                    pstress = chasedPrisoner.GetComponentInChildren<PlayerStress>();
                }

            }
        }

        /// <summary>
        /// This coroutine is being used to add player's stress in certain time interval
        /// </summary>
        /// <returns></returns>
        private IEnumerator GainStressInChase()
        {
            stressLock = true;
            pstress.AddStress(chaseStressLevelRise);
            yield return new WaitForSecondsRealtime(stressGainInterval);
            stressLock = false;
        }

        /// <summary>
        /// How long to wait when checking whether the guard left chase
        /// </summary>
        public float lostFocusTime = 4.0f;
        /// <summary>
        /// Used to check how much close is the guard related to the player's position
        /// </summary>
        public float chaseEpsilon = 2.0f;
        /// <summary>
        /// Is the guard currently in chase
        /// </summary>
        private bool isGuardInChase = false;
        /// <summary>
        /// GameObject reference to prisoner in chase
        /// </summary>
        private GameObject chasedPrisoner;
        /// <summary>
        /// How much does the player stress rise when in chase
        /// </summary>
        public float chaseStressLevelRise = 0.2f;
        /// <summary>
        /// The interval after which the stress is increased in player during chase
        /// </summary>
        public float stressGainInterval = 1.0f;
        /// <summary>
        /// Can the guard even apply stress
        /// </summary>
        private bool canApplyStress = false;
        private bool stressLock = false;
        private PlayerStress pstress;
    }
}