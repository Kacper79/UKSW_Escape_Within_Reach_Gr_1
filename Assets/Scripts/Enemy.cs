using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarzadzanie statystykami wroga, w tym jego zdrowiem i blokowaniem obrazen.
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Maksymalna ilosc zdrowia wroga.
    /// </summary>
    private const int MAX_HP = 25;

    /// <summary>
    /// Pasek zdrowia, ktory jest uzywany do wizualizacji stanu zdrowia wroga.
    /// </summary>
    [SerializeField] private EnemyHealthBar health_bar;

    /// <summary>
    /// Aktualna ilosc zdrowia wroga.
    /// </summary>
    private int current_hp = MAX_HP;

    /// <summary>
    /// Flaga informujaca, czy wrog ma aktywowana blokade.
    /// </summary>
    private bool is_block_up = false;

    /// <summary>
    /// Inicjalizuje wroga, uruchamiajac powtarzajace sie zmiane wartosci blokady.
    /// </summary>
    private void Start()
    {
        InvokeRepeating(nameof(ChangeBlockValue), 1.0f, 1.0f);
    }

    /// <summary>
    /// Zmienia stan blokady wroga (blokada aktywuje sie i deaktywuje cyklicznie).
    /// </summary>
    public void ChangeBlockValue()
    {
        is_block_up = !is_block_up;
    }

    /// <summary>
    /// Zmniejsza zdrowie wroga o okreslona ilosc obrazen.
    /// </summary>
    /// <param name="amount">Ilosc obrazen, ktore wrog otrzymuje.</param>
    public void TakeDamage(int amount)
    {
        current_hp -= amount;

        if(health_bar != null) health_bar.ChangeBarValue(MAX_HP, current_hp);
    }

    /// <summary>
    /// Zwraca informacje, czy wrog ma aktywowana blokade.
    /// </summary>
    /// <returns>True, jesli blokada jest aktywna, w przeciwnym razie false.</returns>
    public bool GetBlockUp()
    {
        return is_block_up;
    }

    /// <summary>
    /// Zwraca aktualna ilosc zdrowia wroga.
    /// </summary>
    /// <returns>Aktualna ilosc zdrowia wroga.</returns>
    public int GetHp()
    {
        return current_hp;
    }
}
