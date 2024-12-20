using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const int MAX_HP = 100;

    [SerializeField] private EnemyHealthBar health_bar;

    private int current_hp = MAX_HP;
    private bool is_block_up = false;

    private void Start()
    {
        InvokeRepeating(nameof(ChangeBlockValue), 1.0f, 1.0f);
    }

    private void ChangeBlockValue()
    {
        is_block_up = !is_block_up;
    }

    public void TakeDamage(int amount)
    {
        current_hp -= amount;

        health_bar.ChangeBarValue(MAX_HP, current_hp);
    }

    public bool GetBlockUp()
    {
        return is_block_up;
    }

    public int GetHp()
    {
        return current_hp;
    }
}
