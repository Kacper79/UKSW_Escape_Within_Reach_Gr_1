using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_card_graphic_SO", menuName = "Scriptable Objects/CardGraphic")]
/// <summary>
/// Klasa reprezentuj¹ca dane graficzne kart, przechowuj¹ce ró¿ne obrazy powi¹zane z kartami.
/// U¿ywana w systemie kart gry w celu przypisania odpowiednich grafik do kart.
/// </summary>
public class CardGraphicSO : ScriptableObject
{
    /// <summary>
    /// Tablica sprite'ów reprezentuj¹cych czarne figury (np. karty w kolorze czarnym, takie jak trefle i pik).
    /// </summary>
    public Sprite[] czarne_figury;

    /// <summary>
    /// Tablica sprite'ów reprezentuj¹cych czerwone figury (np. karty w kolorze czerwonym, takie jak serca i karo).
    /// </summary>
    public Sprite[] czerwone_figury;

    /// <summary>
    /// Tablica sprite'ów reprezentuj¹cych kolory kart, takie jak t³o lub inne graficzne elementy.
    /// </summary>
    public Sprite[] kolory;
}
