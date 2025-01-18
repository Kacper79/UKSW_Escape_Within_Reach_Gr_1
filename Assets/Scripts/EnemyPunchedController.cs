using Assets.Scripts.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarz�dzanie reakcj� wroga na ciosy, w tym blokowanie cios�w, 
/// zadawanie obra�e�, oraz sprawdzanie, czy wr�g zgin��.
/// </summary>
public class EnemyPunchedController : MonoBehaviour, IAttackable
{
    /// <summary>
    /// Maksymalny k�t (w stopniach), w jakim wr�g mo�e zablokowa� cios z uniesion� gard�.
    /// </summary>
    private const float MAX_HIT_EULER_DEGREE_WITH_GUARD_UP = 45.0f;

    /// <summary>
    /// Kontroler wroga, kt�ry zarz�dza jego stanem zdrowia i blokowaniem cios�w.
    /// </summary>
    [SerializeField] private Enemy enemy_controller;

    /// <summary>
    /// Okre�la, czy wr�g jest cz�ci� zadania (quest).
    /// </summary>
    [SerializeField] private bool is_quest_enemy;

    /// <summary>
    /// Obs�uguje cios zadany przez gracza lub inny obiekt. Sprawdza, czy cios zosta� zablokowany.
    /// Je�li nie, zadaje obra�enia, a nast�pnie sprawdza, czy wr�g umar�.
    /// </summary>
    /// <param name="amount">Ilo�� zadanych obra�e�.</param>
    /// <param name="damage_dealer_position">Pozycja �r�d�a zadaj�cego obra�enia.</param>
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
    /// Sprawdza, czy wr�g ma uniesion� gard�.
    /// </summary>
    /// <returns>True, je�li wr�g blokuje ciosy, false w przeciwnym przypadku.</returns>
    bool IAttackable.GetIsGuardUp()
    {
        return enemy_controller.GetBlockUp();
    }

    /// <summary>
    /// Zadaje obra�enia wrogowi.
    /// </summary>
    /// <param name="amount">Ilo�� obra�e� do zadania.</param>
    void IAttackable.TakeDamage(int amount)
    {
        enemy_controller.TakeDamage(amount);
    }

    /// <summary>
    /// Pobiera aktualn� ilo�� punkt�w zdrowia wroga.
    /// </summary>
    /// <returns>Aktualna ilo�� punkt�w zdrowia wroga.</returns>
    int IAttackable.GetHp()
    {
        return enemy_controller.GetHp();
    }

    /// <summary>
    /// Obs�uguje �mier� wroga. Je�li wr�g jest cz�ci� zadania, ko�czy zadanie. 
    /// Nast�pnie niszczy obiekt wroga.
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
    /// Sprawdza, czy wr�g zgin�� na podstawie jego punkt�w zdrowia.
    /// </summary>
    private void CheckIfDead()
    {
        if (((IAttackable)this).GetHp() < 0)
        {
            ((IAttackable)this).Die();
        }
    }

    /// <summary>
    /// Sprawdza, czy cios zosta� zablokowany przez wroga. Blokada jest skuteczna tylko w okre�lonym k�cie
    /// (w stopniach) w zale�no�ci od kierunku, z kt�rego nadchodzi cios.
    /// </summary>
    /// <param name="damage_dealer_position">Pozycja, z kt�rej nadchodzi cios.</param>
    /// <returns>True, je�li cios zosta� zablokowany, false w przeciwnym przypadku.</returns>
    private bool DidBlockPunch(Vector3 damage_dealer_position)
    {
        return ((IAttackable)this).GetIsGuardUp() &&
               (Mathf.Abs(180.0f - (Vector3.Angle(transform.forward, transform.position - damage_dealer_position))) < MAX_HIT_EULER_DEGREE_WITH_GUARD_UP);
    }
}
