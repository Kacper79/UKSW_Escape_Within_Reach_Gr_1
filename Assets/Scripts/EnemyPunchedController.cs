using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPunchedController : MonoBehaviour, IAttackable
{
    private const float MAX_HIT_EULER_DEGREE_WITH_GUARD_UP = 45.0f;

    [SerializeField] private Enemy enemy_controller;

    void IAttackable.Punched(int amount, Vector3 damage_dealer_position)
    {
        if (CanGetDamaged(damage_dealer_position))
        {
            ((IAttackable)this).TakeDamage(amount);
        }

        CheckIfDead();
    }

    bool IAttackable.GetIsGuardUp()
    {
        return enemy_controller.GetBlockUp();
    }

    void IAttackable.TakeDamage(int amount)
    {
        enemy_controller.TakeDamage(amount);
    }

    int IAttackable.GetHp()
    {
        return enemy_controller.GetHp();
    }

    void IAttackable.Die()
    {
        Debug.Log("enemy died");
        Destroy(this.gameObject);
    }

    private void CheckIfDead()
    {
        if(((IAttackable)this).GetHp() < 0)
        {
            ((IAttackable)this).Die();
        }
    }
    
    private bool CanGetDamaged(Vector3 damage_dealer_position)
    {
        return true;//((IAttackable)this).GetIsGuardUp() &&
               //(Vector3.Angle(transform.forward, transform.position - damage_dealer_position) < MAX_HIT_EULER_DEGREE_WITH_GUARD_UP);
    }
}
