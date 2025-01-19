using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reprezentuje pojedyncz� kart� w grze.
/// Przechowuje warto�ci figury i koloru oraz zarz�dza grafik� karty.
/// </summary>
public class Card : MonoBehaviour
{
    [SerializeField] public CardGraphic card_graphic;

    private int figura;
    private int kolor;

    /// <summary>
    /// Inicjalizuje warto�ci figury i koloru karty oraz ustawia grafik�.
    /// </summary>
    /// <param name="figura_">Warto�� figury karty.</param>
    /// <param name="kolor_">Warto�� koloru karty.</param>
    public void InitializeValues(int figura_, int kolor_)
    {
        figura = figura_;
        kolor = kolor_;
        card_graphic.InitializeGraphic(figura, kolor);
    }

    /// <summary>
    /// Resetuje transform karty do domy�lnej pozycji i rotacji.
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
    /// Zwraca warto�� figury karty.
    /// </summary>
    public int GetValue()
    {
        return figura;
    }

    /// <summary>
    /// Ustawia warto�� figury karty.
    /// </summary>
    /// <param name="i">Nowa warto�� figury.</param>
    public void SetValue(int i)
    {
        figura = i;
    }
}
