using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interfejs dla obiektów, które mog¹ byæ atakowane w grze.
/// Zawiera metody odpowiedzialne za otrzymywanie obra¿eñ, sprawdzanie stanu obrony i ¿ycia.
/// </summary>
public interface IAttackable
{
    /// <summary>
    /// Metoda wywo³ywana, gdy obiekt otrzymuje cios.
    /// </summary>
    /// <param name="amount">Iloœæ obra¿eñ zadanych przez cios.</param>
    /// <param name="damage_dealer_position">Pozycja Ÿród³a obra¿eñ, np. pozycja gracza, który zada³ cios.</param>
    public void Punched(int amount, Vector3 damage_dealer_position);

    /// <summary>
    /// Zwraca, czy obiekt ma aktywn¹ obronê (np. tarczê, blok).
    /// </summary>
    /// <returns>True, jeœli obiekt ma aktywn¹ obronê; w przeciwnym razie false.</returns>
    public bool GetIsGuardUp();

    /// <summary>
    /// Metoda, która zadaje obra¿enia obiektowi.
    /// </summary>
    /// <param name="amount">Iloœæ zadanych obra¿eñ.</param>
    public void TakeDamage(int amount);

    /// <summary>
    /// Zwraca aktualn¹ iloœæ punktów ¿ycia obiektu.
    /// </summary>
    /// <returns>Aktualna iloœæ HP obiektu.</returns>
    public int GetHp();

    /// <summary>
    /// Metoda wywo³ywana, gdy obiekt umiera.
    /// </summary>
    public void Die();
}
