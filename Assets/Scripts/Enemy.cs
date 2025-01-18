using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarz¹dzanie statystykami wroga, w tym jego zdrowiem i blokowaniem obra¿eñ.
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Maksymalna iloœæ zdrowia wroga.
    /// </summary>
    private const int MAX_HP = 25;

    /// <summary>
    /// Pasek zdrowia, który jest u¿ywany do wizualizacji stanu zdrowia wroga.
    /// </summary>
    [SerializeField] private EnemyHealthBar health_bar;

    /// <summary>
    /// Aktualna iloœæ zdrowia wroga.
    /// </summary>
    private int current_hp = MAX_HP;

    /// <summary>
    /// Flaga informuj¹ca, czy wróg ma aktywowan¹ blokadê.
    /// </summary>
    private bool is_block_up = false;

    /// <summary>
    /// Inicjalizuje wroga, uruchamiaj¹c powtarzaj¹c¹ siê zmianê wartoœci blokady.
    /// </summary>
    private void Start()
    {
        InvokeRepeating(nameof(ChangeBlockValue), 1.0f, 1.0f);
    }

    /// <summary>
    /// Zmienia stan blokady wroga (blokada aktywuje siê i deaktywuje cyklicznie).
    /// </summary>
    public void ChangeBlockValue()
    {
        is_block_up = !is_block_up;
    }

    /// <summary>
    /// Zmniejsza zdrowie wroga o okreœlon¹ iloœæ obra¿eñ.
    /// </summary>
    /// <param name="amount">Iloœæ obra¿eñ, które wróg otrzymuje.</param>
    public void TakeDamage(int amount)
    {
        current_hp -= amount;

        // Aktualizuje pasek zdrowia wroga na podstawie jego obecnego zdrowia
        health_bar.ChangeBarValue(MAX_HP, current_hp);
    }

    /// <summary>
    /// Zwraca informacjê, czy wróg ma aktywowan¹ blokadê.
    /// </summary>
    /// <returns>True, jeœli blokada jest aktywna, w przeciwnym razie false.</returns>
    public bool GetBlockUp()
    {
        return is_block_up;
    }

    /// <summary>
    /// Zwraca aktualn¹ iloœæ zdrowia wroga.
    /// </summary>
    /// <returns>Aktualna iloœæ zdrowia wroga.</returns>
    public int GetHp()
    {
        return current_hp;
    }
}
