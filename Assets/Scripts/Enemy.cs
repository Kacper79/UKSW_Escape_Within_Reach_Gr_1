using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int hp = 100;
    private bool is_block_up = false;

    private void Start()
    {
        InvokeRepeating("ChangeBlockValue", 1.0f, 1.0f);
    }

    private void ChangeBlockValue()
    {
        is_block_up = !is_block_up;
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        Debug.Log("Ugh, got hit, my hp is " + hp);
    }

    public bool GetBlockUp()
    {
        return is_block_up;
    }

    public int GetHp()
    {
        return hp;
    }
}
