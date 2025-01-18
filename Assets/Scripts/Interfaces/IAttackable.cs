using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interfejs dla obiekt�w, kt�re mog� by� atakowane w grze.
/// Zawiera metody odpowiedzialne za otrzymywanie obra�e�, sprawdzanie stanu obrony i �ycia.
/// </summary>
public interface IAttackable
{
    /// <summary>
    /// Metoda wywo�ywana, gdy obiekt otrzymuje cios.
    /// </summary>
    /// <param name="amount">Ilo�� obra�e� zadanych przez cios.</param>
    /// <param name="damage_dealer_position">Pozycja �r�d�a obra�e�, np. pozycja gracza, kt�ry zada� cios.</param>
    public void Punched(int amount, Vector3 damage_dealer_position);

    /// <summary>
    /// Zwraca, czy obiekt ma aktywn� obron� (np. tarcz�, blok).
    /// </summary>
    /// <returns>True, je�li obiekt ma aktywn� obron�; w przeciwnym razie false.</returns>
    public bool GetIsGuardUp();

    /// <summary>
    /// Metoda, kt�ra zadaje obra�enia obiektowi.
    /// </summary>
    /// <param name="amount">Ilo�� zadanych obra�e�.</param>
    public void TakeDamage(int amount);

    /// <summary>
    /// Zwraca aktualn� ilo�� punkt�w �ycia obiektu.
    /// </summary>
    /// <returns>Aktualna ilo�� HP obiektu.</returns>
    public int GetHp();

    /// <summary>
    /// Metoda wywo�ywana, gdy obiekt umiera.
    /// </summary>
    public void Die();
}
