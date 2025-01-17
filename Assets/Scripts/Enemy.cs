using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const int MAX_HP = 25;

    [SerializeField] private EnemyHealthBar health_bar;

    private int current_hp = MAX_HP;
    private bool is_block_up = false;

    private void Start()
    {
        InvokeRepeating(nameof(ChangeBlockValue), 1.0f, 1.0f);
    }

    public void ChangeBlockValue()
    {
        is_block_up = !is_block_up;
    }

    public void TakeDamage(int amount)
    {
        current_hp -= amount;

        if(health_bar != null) health_bar.ChangeBarValue(MAX_HP, current_hp);
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
