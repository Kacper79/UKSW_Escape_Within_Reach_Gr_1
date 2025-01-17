using Assets.Scripts.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PlayerRelated
{
    /// <summary>
    /// This class is being used to store player's health state and taking damage
    /// </summary>
    public class PlayerAttackAbsorber : MonoBehaviour, IAttackable, ISaveable
    {
        void Start()
        {
            SaveManager.Instance.saveablesGO.Add(this);
        }

        /// <summary>
        /// Call this function to change player's state when the player is supposed to die
        /// </summary>
        public void Die()
        {
            if(is_in_fight_tournament) fightManager.PlayerDiedInTournament();
            GetComponent<DeathScript>().TeleportToInfirmary();
        }

        /// <summary>
        /// Returns the player's current HP points
        /// </summary>
        /// <returns></returns>
        public int GetHp()
        {
            return currentHP;
        }

        /// <summary>
        /// Returns whether the playe's guard is being held
        /// </summary>
        /// <returns></returns>
        public bool GetIsGuardUp()
        {
            return playerAttackingController.is_blocking;
        }

        /// <summary>
        /// This function is being called whenever player is taking damage.
        /// Handles punches or takes player HP depending on the punch.
        /// </summary>
        /// <param name="amount">amount of HP points to be decreased when the punch lands</param>
        /// <param name="damage_dealer_position">the position of the punching player</param>
        public void Punched(int amount, Vector3 damage_dealer_position)
        {
            if (!DidBlockPunch(damage_dealer_position))
            {
                Debug.Log("Damaged");
                ((IAttackable)this).TakeDamage(amount);
            }
            else
            {
                Debug.Log("Blovcked");
            }
        }

        /// <summary>
        /// Decreases player health by a supplied amount
        /// Handles dying if the health is too low
        /// </summary>
        /// <param name="amount">amount of HP points to be decreased</param>
        public void TakeDamage(int amount)
        {
            if(currentHP - amount <= 0)
            {
                Die();
            } else
            {
                currentHP -= amount;
            }
        }

        /// <summary>
        /// Increases player health by a supplied amount
        /// </summary>
        /// <param name="amount">amount of HP points to be healed</param>
        public void Heal(int amount)
        {
            if (currentHP + amount > MaxHP) currentHP = MaxHP;
            else currentHP += amount;
        }

        /// <summary>
        /// This is a utility function that determines whether the angle between player and the puncher is sufficient to block damage
        /// </summary>
        /// <param name="damage_dealer_position">the position of the punching player</param>
        /// <returns>Boolean value telling if the punch has been blocked by the guard</returns>
        private bool DidBlockPunch(Vector3 damage_dealer_position)
        {
            return ((IAttackable)this).GetIsGuardUp() &&
                   (Mathf.Abs(180.0f - (Vector3.Angle(transform.forward, transform.position - damage_dealer_position))) < MAX_HIT_EULER_DEGREE_WITH_GUARD_UP);
        }

        /// <summary>
        /// This function is being used to save player health to the save file
        /// </summary>
        /// <param name="saveData">Mutable save data struct to save data to</param>
        public void Save(ref SaveData saveData)
        {
            saveData.playerCurrentHP = currentHP;
            saveData.playerMaxHP = MaxHP;
        }

        /// <summary>
        /// This function is being used to load player health from the save file
        /// </summary>
        /// <param name="saveData">Save data struct to load data from</param>
        public void Load(SaveData saveData)
        {
            currentHP = saveData.playerCurrentHP;
            MaxHP = saveData.playerMaxHP;
        }

        public void SetIsInFightTournament(bool b)
        {
            is_in_fight_tournament = b;
        }

        private bool is_in_fight_tournament = false;

        private int currentHP = DefaultMaxHO;
        public int MaxHP = DefaultMaxHO;
        public const int DefaultMaxHO = 25;
        private const float MAX_HIT_EULER_DEGREE_WITH_GUARD_UP = 45.0f;

        [SerializeField] private PlayerAttackingController playerAttackingController;
        [SerializeField] private FightManager fightManager;
    }
}