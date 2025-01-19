using Assets.Scripts.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarz¹dzanie reakcj¹ wroga na ciosy, w tym blokowanie ciosów, 
/// zadawanie obra¿eñ, oraz sprawdzanie, czy wróg zgin¹³.
/// </summary>
public class EnemyPunchedController : MonoBehaviour, IAttackable
{
    /// <summary>
    /// Maksymalny k¹t (w stopniach), w jakim wróg mo¿e zablokowaæ cios z uniesion¹ gard¹.
    /// </summary>
    private const float MAX_HIT_EULER_DEGREE_WITH_GUARD_UP = 45.0f;

    /// <summary>
    /// Kontroler wroga, który zarz¹dza jego stanem zdrowia i blokowaniem ciosów.
    /// </summary>
    [SerializeField] private Enemy enemy_controller;

    /// <summary>
    /// Okreœla, czy wróg jest czêœci¹ zadania (quest).
    /// </summary>
    [SerializeField] private bool is_quest_enemy;

    /// <summary>
    /// Obs³uguje cios zadany przez gracza lub inny obiekt. Sprawdza, czy cios zosta³ zablokowany.
    /// Jeœli nie, zadaje obra¿enia, a nastêpnie sprawdza, czy wróg umar³.
    /// </summary>
    /// <param name="amount">Iloœæ zadanych obra¿eñ.</param>
    /// <param name="damage_dealer_position">Pozycja Ÿród³a zadaj¹cego obra¿enia.</param>
    void IAttackable.Punched(int amount, Vector3 damage_dealer_position)
    {
        if (!DidBlockPunch(damage_dealer_position))
        {
            Debug.Log("Damaged");
            ((IAttackable)this).TakeDamage(amount);
        }
        else
        {
            Debug.Log("Blocked");
        }

        CheckIfDead();
    }

    /// <summary>
    /// Sprawdza, czy wróg ma uniesion¹ gardê.
    /// </summary>
    /// <returns>True, jeœli wróg blokuje ciosy, false w przeciwnym przypadku.</returns>
    bool IAttackable.GetIsGuardUp()
    {
        return enemy_controller.GetBlockUp();
    }

    /// <summary>
    /// Zadaje obra¿enia wrogowi.
    /// </summary>
    /// <param name="amount">Iloœæ obra¿eñ do zadania.</param>
    void IAttackable.TakeDamage(int amount)
    {
        enemy_controller.TakeDamage(amount);
    }

    /// <summary>
    /// Pobiera aktualn¹ iloœæ punktów zdrowia wroga.
    /// </summary>
    /// <returns>Aktualna iloœæ punktów zdrowia wroga.</returns>
    int IAttackable.GetHp()
    {
        return enemy_controller.GetHp();
    }

    /// <summary>
    /// Obs³uguje œmieræ wroga. Jeœli wróg jest czêœci¹ zadania, koñczy zadanie. 
    /// Nastêpnie niszczy obiekt wroga.
    /// </summary>
    void IAttackable.Die()
    {
        Debug.Log("Dies");
        GlobalEvents.FireOnBeatingEnemyInATournament(this);
        if (transform.parent.GetComponentInChildren<FightNPC>() != null)
        {
            transform.parent.GetComponentInChildren<FightNPC>().EndFight();
        }

        if (is_quest_enemy)
        {
            QuestManager.Instance.MarkQuestCompleted(7);
        }

        Destroy(this.transform.parent.gameObject);
    }

    /// <summary>
    /// Sprawdza, czy wróg zgin¹³ na podstawie jego punktów zdrowia.
    /// </summary>
    private void CheckIfDead()
    {
        if (((IAttackable)this).GetHp() < 0)
        {
            ((IAttackable)this).Die();
        }
    }

    /// <summary>
    /// Sprawdza, czy cios zosta³ zablokowany przez wroga. Blokada jest skuteczna tylko w okreœlonym k¹cie
    /// (w stopniach) w zale¿noœci od kierunku, z którego nadchodzi cios.
    /// </summary>
    /// <param name="damage_dealer_position">Pozycja, z której nadchodzi cios.</param>
    /// <returns>True, jeœli cios zosta³ zablokowany, false w przeciwnym przypadku.</returns>
    private bool DidBlockPunch(Vector3 damage_dealer_position)
    {
        return ((IAttackable)this).GetIsGuardUp() &&
               (Mathf.Abs(180.0f - (Vector3.Angle(transform.forward, transform.position - damage_dealer_position))) < MAX_HIT_EULER_DEGREE_WITH_GUARD_UP);
    }
}
