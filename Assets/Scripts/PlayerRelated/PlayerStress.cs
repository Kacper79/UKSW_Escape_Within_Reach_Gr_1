using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PlayerRelated
{
    public class PlayerStress : MonoBehaviour
    {
        public void AddStress(float stress)
        {
            if (maxStressLevel + stress < maxStressLevel) stressLevel += stress;
            else {
                GetComponent<DeathScript>().TeleportToInfirmary();
                stressLevel = 0.0f;
            }
        }

        public void RemoveStress(float stress)
        {
            if (stressLevel - stress >= 0) stressLevel -= stress;
        }

        public float stressLevel;
        public float maxStressLevel;

    }
}