using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_card_graphic_SO", menuName = "Scriptable Objects/CardGraphic")]
/// <summary>
/// Klasa reprezentujaca dane graficzne kart, przechowujace rozne obrazy powiazane z kartami.
/// Uzywana w systemie kart gry w celu przypisania odpowiednich grafik do kart.
/// </summary>
public class CardGraphicSO : ScriptableObject
{
    /// <summary>
    /// Tablica sprite'ow reprezentujacych czarne figury (np. karty w kolorze czarnym, takie jak trefle i pik).
    /// </summary>
    public Sprite[] czarne_figury;

    /// <summary>
    /// Tablica sprite'ow reprezentujacych czerwone figury (np. karty w kolorze czerwonym, takie jak serca i karo).
    /// </summary>
    public Sprite[] czerwone_figury;

    /// <summary>
    /// Tablica sprite'ow reprezentujacych kolory kart, takie jak tlo lub inne graficzne elementy.
    /// </summary>
    public Sprite[] kolory;
}
