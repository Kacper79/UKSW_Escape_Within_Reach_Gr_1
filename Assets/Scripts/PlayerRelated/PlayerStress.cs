using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PlayerRelated
{
    /// <summary>
    /// This script contains 
    /// </summary>
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

        /// <summary>
        /// Use this method to increase the player's stress by a supplied amount.
        /// Handles dying from too much stress.
        /// </summary>
        /// <param name="stress">the amount of stress for the player to increase</param>
        public void AddStress(float stress)
        {
            if (stressLevel + stress < maxStressLevel) stressLevel += stress;
            else {
                GetComponent<DeathScript>().TeleportToInfirmary();
                stressLevel = 0.0f;
            }
        }

        /// <summary>
        /// Use this method to decrease the player's stress by a supplied amount.
        /// </summary>
        /// <param name="stress">the amount of stress for the player to decrease</param>
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

        /// <summary>
        /// This callback function is called on each passing second to check whether the player is outside his cell during curfew
        /// </summary>
        /// <param name="sender">the class calling this callback</param>
        /// <param name="e">time in minutes</param>
        private void OnTimeChange(object sender, GlobalEvents.OnChangingTimeArgs e)
        {
            if(e.minutes > forbiddenTimeMinutes && !isResidingInCell)
            {
                AddStress(forbiddenTimeStressRise);
            }
        }

        /// <summary>
        /// This function is being used to save player's stress to the save file
        /// </summary>
        /// <param name="saveData">Mutable save data struct to save data to</param>
        public void Save(ref SaveData saveData)
        {
            saveData.playerStressLevel = stressLevel;
            saveData.playerResidingCell = isResidingInCell;
        }

        /// <summary>
        /// This function is being used to load player's stress from the save file
        /// </summary>
        /// <param name="saveData">Ssave data struct to load data from</param>
        public void Load(SaveData saveData)
        {
             stressLevel = saveData.playerStressLevel;
             isResidingInCell = saveData.playerResidingCell;
        }

        private InventoryManager inventoryManager;
        [SerializeField] private Item cigsItemPrefab;

        /// <summary>
        /// The player's current stress level
        /// </summary>
        private float stressLevel;
        public float CurrentStressLevel => stressLevel;
        /// <summary>
        /// The player's max stress level after which the player is killed
        /// </summary>
        public float maxStressLevel;
        /// <summary>
        /// How much does the stress level fall when smoking cigs
        /// </summary>
        public float cigPackStressFall;
        /// <summary>
        /// Is player currently residing in his cell
        /// </summary>
        public bool isResidingInCell = false;
        /// <summary>
        /// How much does the player's stress level rise whenever the player is outside his cell during curfew
        /// </summary>
        public float forbiddenTimeStressRise;
        /// <summary>
        /// The time (in minutes) after which the curfew starts
        /// </summary>
        public const int forbiddenTimeMinutes = 20 * 60;
    }
}