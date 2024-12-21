using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class RestrictedZone : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out DeathScript dsc))
            {
                Debug.Log($"The restricted zone has been entered by {other.gameObject.name}");
                dsc.wantToLeaveZone = false;
                dsc.restrictedZoneName = gameObject.name;
                dsc.walksInRestrictedZone = true;
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out DeathScript dsc) && dsc.wantToLeaveZone) dsc.wantToLeaveZone = false;
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out DeathScript dsc))
            {
                Debug.Log("The restricted zone has been left");
                dsc.wantToLeaveZone = true;
                StartCoroutine(TryLeaveRestrictedState(dsc));
            }
        }

        public IEnumerator TryLeaveRestrictedState(DeathScript dsc)
        {
            yield return new WaitForSecondsRealtime(1.0f);
            if (dsc.wantToLeaveZone)
            {
                Debug.Log("No longer walking in restricted space");
                dsc.restrictedZoneName = "";
                dsc.walksInRestrictedZone = false;
                if (dsc.guardsChasing.Count > 0)
                {
                    dsc.guardsChasing.ForEach(guard => StartCoroutine(guard.LoseChaseFocus()));
                }
            }
        }
    }
}