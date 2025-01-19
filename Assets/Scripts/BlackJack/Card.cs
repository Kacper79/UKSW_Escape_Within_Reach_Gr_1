using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reprezentuje pojedyncz¹ kartê w grze.
/// Przechowuje wartoœci figury i koloru oraz zarz¹dza grafik¹ karty.
/// </summary>
public class Card : MonoBehaviour
{
    [SerializeField] public CardGraphic card_graphic;

    private int figura;
    private int kolor;

    /// <summary>
    /// Inicjalizuje wartoœci figury i koloru karty oraz ustawia grafikê.
    /// </summary>
    /// <param name="figura_">Wartoœæ figury karty.</param>
    /// <param name="kolor_">Wartoœæ koloru karty.</param>
    public void InitializeValues(int figura_, int kolor_)
    {
        figura = figura_;
        kolor = kolor_;
        card_graphic.InitializeGraphic(figura, kolor);
    }

    /// <summary>
    /// Resetuje transform karty do domyœlnej pozycji i rotacji.
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
    /// Zwraca wartoœæ figury karty.
    /// </summary>
    public int GetValue()
    {
        return figura;
    }

    /// <summary>
    /// Ustawia wartoœæ figury karty.
    /// </summary>
    /// <param name="i">Nowa wartoœæ figury.</param>
    public void SetValue(int i)
    {
        figura = i;
    }
}
