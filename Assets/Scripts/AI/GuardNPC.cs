using Assets.Scripts.PlayerRelated;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI
{
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

        private IEnumerator GainStressInChase()
        {
            stressLock = true;
            pstress.AddStress(chaseStressLevelRise);
            yield return new WaitForSecondsRealtime(stressGainInterval);
            stressLock = false;
        }

        private float lostFocusTime = 4.0f;
        private float chaseEpsilon = 2.0f;
        private bool isGuardInChase = false;
        private GameObject chasedPrisoner;

        private float chaseStressLevelRise = 0.2f;
        private float stressGainInterval = 1.0f;
        private bool canApplyStress = false;
        private bool stressLock = false;
        private PlayerStress pstress;
    }
}