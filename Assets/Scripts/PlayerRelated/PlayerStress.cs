using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PlayerRelated
{
    public class PlayerStress : MonoBehaviour, ISaveable
    {
        void Start()
        {
            inventoryManager = GetComponentInChildren<InventoryManager>();
            SaveManager.Instance.saveablesGO.Add(this);
        }

        void OnEnable()
        {
            GlobalEvents.OnUseCigs += OnUseCigarettes;
            GlobalEvents.OnChangingTime += OnTimeChange;
        }

        void OnDisable()
        {
            GlobalEvents.OnUseCigs -= OnUseCigarettes;
            GlobalEvents.OnChangingTime -= OnTimeChange;
        }

        public void AddStress(float stress)
        {
            if (stressLevel + stress < maxStressLevel) stressLevel += stress;
            else {
                GetComponent<DeathScript>().TeleportToInfirmary();
                stressLevel = 0.0f;
            }
        }

        public void RemoveStress(float stress)
        {
            if (stressLevel - stress >= 0) stressLevel -= stress;
            else stressLevel = 0;
        }

        private void OnUseCigarettes(object sender, EventArgs e)
        {
            if (inventoryManager.RemoveUsedItem(cigsItemPrefab))
            {
                RemoveStress(cigPackStressFall);
            }
            else Debug.Log("You don't have enough cigs to lower stress");
        }

        private void OnTimeChange(object sender, GlobalEvents.OnChangingTimeArgs e)
        {
            if(e.minutes > forbiddenTimeMinutes && !isResidingInCell)
            {
                AddStress(forbiddenTimeStressRise);
            }
        }

        public void Save(ref SaveData saveData)
        {
            saveData.playerStressLevel = stressLevel;
            saveData.playerResidingCell = isResidingInCell;
        }

        public void Load(SaveData saveData)
        {
             stressLevel = saveData.playerStressLevel;
             isResidingInCell = saveData.playerResidingCell;
        }

        private InventoryManager inventoryManager;
        [SerializeField] private Item cigsItemPrefab;

        public float stressLevel;
        public float maxStressLevel;
        public float cigPackStressFall;
        public bool isResidingInCell = false;
        public float forbiddenTimeStressRise;
        public const int forbiddenTimeMinutes = 20 * 60;
    }
}