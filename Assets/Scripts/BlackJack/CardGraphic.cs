using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zarz¹dza grafik¹ karty, w tym jej awersem, rewersem i ikonami.
/// </summary>
public class CardGraphic : MonoBehaviour
{
    [Header("Grafika Kart")]
    [SerializeField] private CardGraphicSO grafika_kart_SO;

    [Header("Sprite'y karty")]
    [SerializeField] private SpriteRenderer figura_gora;
    [SerializeField] private SpriteRenderer kolor_gora;
    [SerializeField] private SpriteRenderer figura_dol;
    [SerializeField] private SpriteRenderer kolor_dol;
    [SerializeField] private SpriteRenderer obrazek_srodek;

    [Header("Awers/Rewers")]
    [SerializeField] private GameObject obverse;
    [SerializeField] private GameObject reverse;

    /// <summary>
    /// Inicjalizuje grafikê karty na podstawie podanej figury i koloru.
    /// </summary>
    /// <param name="figura">Numer figury karty.</param>
    /// <param name="kolor">Numer koloru karty.</param>
    public void InitializeGraphic(int figura, int kolor)
    {
        kolor_gora.sprite = grafika_kart_SO.kolory[kolor];
        kolor_dol.sprite = grafika_kart_SO.kolory[kolor];
        if (kolor == 0 || kolor == 3)
        {
            figura_gora.sprite = grafika_kart_SO.czarne_figury[figura];
            figura_dol.sprite = grafika_kart_SO.czarne_figury[figura];
            obrazek_srodek.sprite = grafika_kart_SO.czarne_figury[figura];
        }
        else
        {
            figura_gora.sprite = grafika_kart_SO.czerwone_figury[figura];
            figura_dol.sprite = grafika_kart_SO.czerwone_figury[figura];
            obrazek_srodek.sprite = grafika_kart_SO.czerwone_figury[figura];
        }
        ShowReverse();
    }

    /// <summary>
    /// Wyœwietla awers karty.
    /// </summary>
    public void ShowObverse()
    {
        obverse.SetActive(true);
        reverse.SetActive(false);
    }

    /// <summary>
    /// Ukrywa grafikê karty.
    /// </summary>
    public void TurnOffgraphic()
    {
        obverse.SetActive(false);
        reverse.SetActive(false);
    }

    /// <summary>
    /// Wyœwietla rewers karty.
    /// </summary>
    public void ShowReverse()
    {
        reverse.SetActive(true);
        obverse.SetActive(false);
    }
}
