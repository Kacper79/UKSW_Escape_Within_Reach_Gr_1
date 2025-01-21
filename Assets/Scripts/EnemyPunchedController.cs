using Assets.Scripts.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarzadzanie reakcja wroga na ciosy, w tym blokowanie ciosow, 
/// zadawanie obrazen, oraz sprawdzanie, czy wrog zginal.
/// </summary>
public class EnemyPunchedController : MonoBehaviour, IAttackable
{
    /// <summary>
    /// Maksymalny kat (w stopniach), w jakim wrog moze zablokowac cios z uniesiona garda.
    /// </summary>
    private const float MAX_HIT_EULER_DEGREE_WITH_GUARD_UP = 45.0f;

    /// <summary>
    /// Kontroler wroga, ktory zarzadza jego stanem zdrowia i blokowaniem ciosow.
    /// </summary>
    [SerializeField] private Enemy enemy_controller;

    /// <summary>
    /// Okresla, czy wrog jest czescia zadania (quest).
    /// </summary>
    [SerializeField] private bool is_quest_enemy;

    [SerializeField] private GameObject objectToDestroy;

    /// <summary>
    /// Obsluguje cios zadany przez gracza lub inny obiekt. Sprawdza, czy cios zostal zablokowany.
    /// Jesli nie, zadaje obrazenia, a nastepnie sprawdza, czy wrog umarl.
    /// </summary>
    /// <param name="amount">Ilosc zadanych obrazen.</param>
    /// <param name="damage_dealer_position">Pozycja srodka zadajacego obrazenia.</param>
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
    /// Sprawdza, czy wrog ma uniesiona garde.
    /// </summary>
    /// <returns>True, jesli wrog blokuje ciosy, false w przeciwnym przypadku.</returns>
    bool IAttackable.GetIsGuardUp()
    {
        return enemy_controller.GetBlockUp();
    }

    /// <summary>
    /// Zadaje obrazenia wrogowi.
    /// </summary>
    /// <param name="amount">Ilosc obrazen do zadania.</param>
    void IAttackable.TakeDamage(int amount)
    {
        enemy_controller.TakeDamage(amount);
    }

    /// <summary>
    /// Pobiera aktualna ilosc punktow zdrowia wroga.
    /// </summary>
    /// <returns>Aktualna ilosc punktow zdrowia wroga.</returns>
    int IAttackable.GetHp()
    {
        return enemy_controller.GetHp();
    }

    /// <summary>
    /// Obsluguje smierc wroga. Jesli wrog jest czeœcia zadania, konczy zadanie. 
    /// Nastepnie niszczy obiekt wroga.
    /// </summary>
    void IAttackable.Die()
    {
        Debug.Log("Dies");
        if (transform.parent != null && transform.parent.GetComponentInChildren<FightNPC>() != null)
        {
            GlobalEvents.FireOnBeatingEnemyInATournament(this);
            transform.parent.GetComponentInChildren<FightNPC>().EndFight();
        }

        if (is_quest_enemy)
        {
            QuestManager.Instance.MarkQuestCompleted(6);
        }

        Destroy(objectToDestroy);
    }

    /// <summary>
    /// Sprawdza, czy wrog zginal na podstawie jego punktow zdrowia.
    /// </summary>
    private void CheckIfDead()
    {
        if (((IAttackable)this).GetHp() < 0)
        {
            ((IAttackable)this).Die();
        }
    }

    /// <summary>
    /// Sprawdza, czy cios zostal zablokowany przez wroga. Blokada jest skuteczna tylko w okreslonym kacie
    /// (w stopniach) w zaleznosci od kierunku, z ktorego nadchodzi cios.
    /// </summary>
    /// <param name="damage_dealer_position">Pozycja, z ktorej nadchodzi cios.</param>
    /// <returns>True, jesli cios zostal zablokowany, false w przeciwnym przypadku.</returns>
    private bool DidBlockPunch(Vector3 damage_dealer_position)
    {
        return ((IAttackable)this).GetIsGuardUp() &&
               (Mathf.Abs(180.0f - (Vector3.Angle(transform.forward, transform.position - damage_dealer_position))) < MAX_HIT_EULER_DEGREE_WITH_GUARD_UP);
    }
}
