using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za wyœwietlanie i zarz¹dzanie stron¹ znalezion¹ przez gracza.
/// </summary>
public class FoundPageUI : MonoBehaviour
{
    /// <summary>
    /// Pole przechowuj¹ce referencje do komponentów TextMeshProUGUI dla tytu³u i treœci strony.
    /// </summary>
    [Header("Content references")]
    [SerializeField] private TextMeshProUGUI title_TMP;
    [SerializeField] private TextMeshProUGUI content_TMP;

    /// <summary>
    /// Liczba naciœniêæ przycisku interakcji. S³u¿y do zarejestrowania naciœniêæ przycisku E oraz zniszczenia strony po interakcji.
    /// </summary>
    private int times_interaction_button_was_pressed = 0;

    /// <summary>
    /// Uruchamia zdarzenie informuj¹ce o otwarciu UI.
    /// </summary>
    private void Start()
    {
        GlobalEvents.FireOnAnyUIOpen(this);
    }

    /// <summary>
    /// Sprawdza, czy gracz nacisn¹³ klawisz Escape w celu zamkniêcia strony.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CleanUpBeforeAndDestroy();
        }
    }

    /// <summary>
    /// Inicjalizuje stronê, ustawiaj¹c tytu³ i treœæ.
    /// </summary>
    /// <param name="title">Tytu³ strony.</param>
    /// <param name="content">Treœæ strony.</param>
    public void Init(string title, string content)
    {
        title_TMP.text = title;
        content_TMP.text = content;
    }

    /// <summary>
    /// Zamyka stronê, wywo³uj¹c odpowiednie zdarzenia i niszcz¹c obiekt.
    /// </summary>
    private void CleanUpBeforeAndDestroy()
    {
        GlobalEvents.FireOnStoppingReadingPage(this);
        GlobalEvents.FireOnAnyUIClose(this);

        Destroy(this.gameObject);
    }
}
