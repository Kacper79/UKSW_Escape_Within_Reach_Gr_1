using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interfejs dla obiektow, ktore moga byc atakowane w grze.
/// Zawiera metody odpowiedzialne za otrzymywanie obrazen, sprawdzanie stanu obrony i zycia.
/// </summary>
public interface IAttackable
{
    /// <summary>
    /// Metoda wywolywana, gdy obiekt otrzymuje cios.
    /// </summary>
    /// <param name="amount">Ilosc obrazen zadanych przez cios.</param>
    /// <param name="damage_dealer_position">Pozycja srodka obrazen, np. pozycja gracza, ktory zadal cios.</param>
    public void Punched(int amount, Vector3 damage_dealer_position);

    /// <summary>
    /// Zwraca, czy obiekt ma aktywna obrone (np. tarcze, blok).
    /// </summary>
    /// <returns>True, jesli obiekt ma aktywna obrone; w przeciwnym razie false.</returns>
    public bool GetIsGuardUp();

    /// <summary>
    /// Metoda, ktora zadaje obrazenia obiektowi.
    /// </summary>
    /// <param name="amount">Ilosc zadanych obrazen.</param>
    public void TakeDamage(int amount);

    /// <summary>
    /// Zwraca aktualna ilosc punktow zycia obiektu.
    /// </summary>
    /// <returns>Aktualna ilosc HP obiektu.</returns>
    public int GetHp();

    /// <summary>
    /// Metoda wywolywana, gdy obiekt umiera.
    /// </summary>
    public void Die();
}
