using Assets.Scripts.Interfaces;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PlayerRelated
{
    public class PlayerAttackAbsorber : MonoBehaviour, IAttackable, ISaveable
    {
        void Start()
        {
            SaveManager.Instance.saveablesGO.Add(this);
        }

        public void Die()
        {
            if(fightManager != null) fightManager.EndTournament();
            GetComponent<DeathScript>().TeleportToInfirmary();
        }

        public int GetHp()
        {
            return currentHP;
        }

        public bool GetIsGuardUp()
        {
            return playerAttackingController.is_blocking;
        }

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

        public void Heal(int amount)
        {
            if (currentHP + amount > MaxHP) currentHP = MaxHP;
            else currentHP += amount;
        }

        private bool DidBlockPunch(Vector3 damage_dealer_position)
        {
            return ((IAttackable)this).GetIsGuardUp() &&
                   (Mathf.Abs(180.0f - (Vector3.Angle(transform.forward, transform.position - damage_dealer_position))) < MAX_HIT_EULER_DEGREE_WITH_GUARD_UP);
        }

        public void Save(ref SaveData saveData)
        {
            saveData.playerCurrentHP = currentHP;
            saveData.playerMaxHP = MaxHP;
        }

        public void Load(SaveData saveData)
        {
            currentHP = saveData.playerCurrentHP;
            MaxHP = saveData.playerMaxHP;
        }

        private int currentHP = DefaultMaxHO;
        public int MaxHP = DefaultMaxHO;
        public const int DefaultMaxHO = 25;
        private const float MAX_HIT_EULER_DEGREE_WITH_GUARD_UP = 45.0f;

        [SerializeField] private PlayerAttackingController playerAttackingController;
        [SerializeField] private FightManager fightManager;
    }
}