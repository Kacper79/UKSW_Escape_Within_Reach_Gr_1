using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reprezentuje pojedyncza karte w grze.
/// Przechowuje wartosci figury i koloru oraz zarzdza grafika karty.
/// </summary>
public class Card : MonoBehaviour
{
    [SerializeField] public CardGraphic card_graphic;

    private int figura;
    private int kolor;

    /// <summary>
    /// Inicjalizuje wartosci figury i koloru karty oraz ustawia grafike.
    /// </summary>
    /// <param name="figura_">Wartosc figury karty.</param>
    /// <param name="kolor_">Wartosc koloru karty.</param>
    public void InitializeValues(int figura_, int kolor_)
    {
        figura = figura_;
        kolor = kolor_;
        card_graphic.InitializeGraphic(figura, kolor);
    }

    /// <summary>
    /// Resetuje transform karty do domyslnej pozycji i rotacji.
    /// </summary>
    public void ZerowanieTranformu()
    {
        transform.position = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// Zwraca kolor karty.
    /// </summary>
    public int GetColor()
    {
        return kolor;
    }

    /// <summary>
    /// Zwraca wartosc figury karty.
    /// </summary>
    public int GetValue()
    {
        return figura;
    }

    /// <summary>
    /// Ustawia wartosc figury karty.
    /// </summary>
    /// <param name="i">Nowa wartosc figury.</param>
    public void SetValue(int i)
    {
        figura = i;
    }
}
