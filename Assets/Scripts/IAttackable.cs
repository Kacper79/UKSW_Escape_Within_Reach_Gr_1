using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public void Punched(int amount, Vector3 damage_dealer_position);

    public bool GetIsGuardUp();

    public void TakeDamage(int amount);

    public int GetHp();

    public void Die();
}
