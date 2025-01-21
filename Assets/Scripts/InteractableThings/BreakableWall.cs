using Assets.Scripts.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.InteractableThings
{
    public class BreakableWall : MonoBehaviour, IInteractable, ISaveable
    {
        public void AdditionalStuffWhenLookingAtInteractable()
        {
            return;
        }

        public string GetInteractionTooltip()
        {
            plotItemsUsed = playerInventory.GetPlotItemCount;
            if (plotItemsUsed != plotItemsCountToEscape) return $"You need to get all {plotItemsCountToEscape} plot items to work on that wall";
            else return "Press [E] to break wall";
        }

        public void Interact()
        {
            plotItemsUsed = playerInventory.GetPlotItemCount;
            if (plotItemsUsed == plotItemsCountToEscape)
            {
                QuestManager.Instance.MarkQuestCompleted(8);
                Destroy(gameObject);
                Destroy(another_wall_to_destroy);
            }
        }

        // Use this for initialization
        void Start()
        {
            plotItemsUsed = 0;
            SaveManager.Instance.saveablesGO.Add(this);
        }

        public void Save(ref SaveData saveData)
        {
            saveData.breakableWallItemsUsed = plotItemsUsed;
        }

        public void Load(SaveData saveData)
        {
            plotItemsUsed = saveData.breakableWallItemsUsed;
        }

        [SerializeField] private InventoryManager playerInventory;
        private int plotItemsUsed;
        public int plotItemsCountToEscape = 4;
        public GameObject another_wall_to_destroy;
    }
}