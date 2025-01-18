using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za wy�wietlanie i zarz�dzanie stron� znalezion� przez gracza.
/// </summary>
public class FoundPageUI : MonoBehaviour
{
    /// <summary>
    /// Pole przechowuj�ce referencje do komponent�w TextMeshProUGUI dla tytu�u i tre�ci strony.
    /// </summary>
    [Header("Content references")]
    [SerializeField] private TextMeshProUGUI title_TMP;
    [SerializeField] private TextMeshProUGUI content_TMP;

    /// <summary>
    /// Liczba naci�ni�� przycisku interakcji. S�u�y do zarejestrowania naci�ni�� przycisku E oraz zniszczenia strony po interakcji.
    /// </summary>
    private int times_interaction_button_was_pressed = 0;

    /// <summary>
    /// Uruchamia zdarzenie informuj�ce o otwarciu UI.
    /// </summary>
    private void Start()
    {
        GlobalEvents.FireOnAnyUIOpen(this);
    }

    /// <summary>
    /// Sprawdza, czy gracz nacisn�� klawisz Escape w celu zamkni�cia strony.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CleanUpBeforeAndDestroy();
        }
    }

    /// <summary>
    /// Inicjalizuje stron�, ustawiaj�c tytu� i tre��.
    /// </summary>
    /// <param name="title">Tytu� strony.</param>
    /// <param name="content">Tre�� strony.</param>
    public void Init(string title, string content)
    {
        title_TMP.text = title;
        content_TMP.text = content;
    }

    /// <summary>
    /// Zamyka stron�, wywo�uj�c odpowiednie zdarzenia i niszcz�c obiekt.
    /// </summary>
    private void CleanUpBeforeAndDestroy()
    {
        GlobalEvents.FireOnStoppingReadingPage(this);
        GlobalEvents.FireOnAnyUIClose(this);

        Destroy(this.gameObject);
    }
}
