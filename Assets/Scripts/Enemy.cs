using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za zarz�dzanie statystykami wroga, w tym jego zdrowiem i blokowaniem obra�e�.
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Maksymalna ilo�� zdrowia wroga.
    /// </summary>
    private const int MAX_HP = 25;

    /// <summary>
    /// Pasek zdrowia, kt�ry jest u�ywany do wizualizacji stanu zdrowia wroga.
    /// </summary>
    [SerializeField] private EnemyHealthBar health_bar;

    /// <summary>
    /// Aktualna ilo�� zdrowia wroga.
    /// </summary>
    private int current_hp = MAX_HP;

    /// <summary>
    /// Flaga informuj�ca, czy wr�g ma aktywowan� blokad�.
    /// </summary>
    private bool is_block_up = false;

    /// <summary>
    /// Inicjalizuje wroga, uruchamiaj�c powtarzaj�c� si� zmian� warto�ci blokady.
    /// </summary>
    private void Start()
    {
        InvokeRepeating(nameof(ChangeBlockValue), 1.0f, 1.0f);
    }

    /// <summary>
    /// Zmienia stan blokady wroga (blokada aktywuje si� i deaktywuje cyklicznie).
    /// </summary>
    public void ChangeBlockValue()
    {
        is_block_up = !is_block_up;
    }

    /// <summary>
    /// Zmniejsza zdrowie wroga o okre�lon� ilo�� obra�e�.
    /// </summary>
    /// <param name="amount">Ilo�� obra�e�, kt�re wr�g otrzymuje.</param>
    public void TakeDamage(int amount)
    {
        current_hp -= amount;

        if(health_bar != null) health_bar.ChangeBarValue(MAX_HP, current_hp);
    }

    /// <summary>
    /// Zwraca informacj�, czy wr�g ma aktywowan� blokad�.
    /// </summary>
    /// <returns>True, je�li blokada jest aktywna, w przeciwnym razie false.</returns>
    public bool GetBlockUp()
    {
        return is_block_up;
    }

    /// <summary>
    /// Zwraca aktualn� ilo�� zdrowia wroga.
    /// </summary>
    /// <returns>Aktualna ilo�� zdrowia wroga.</returns>
    public int GetHp()
    {
        return current_hp;
    }
}
