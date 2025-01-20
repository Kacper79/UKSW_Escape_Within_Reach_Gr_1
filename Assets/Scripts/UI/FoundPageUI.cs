using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za wyswietlanie i zarzadzanie strona znaleziona przez gracza.
/// </summary>
public class FoundPageUI : MonoBehaviour
{
    /// <summary>
    /// Pole przechowujace referencje do komponentow TextMeshProUGUI dla tytulu i treœci strony.
    /// </summary>
    [Header("Content references")]
    [SerializeField] private TextMeshProUGUI title_TMP;
    [SerializeField] private TextMeshProUGUI content_TMP;

    /// <summary>
    /// Liczba nacisniec przycisku interakcji. Sluzy do zarejestrowania nacisniec przycisku E oraz zniszczenia strony po interakcji.
    /// </summary>
    private int times_interaction_button_was_pressed = 0;

    /// <summary>
    /// Uruchamia zdarzenie informujace o otwarciu UI.
    /// </summary>
    private void Start()
    {
        GlobalEvents.FireOnAnyUIOpen(this);
    }

    /// <summary>
    /// Sprawdza, czy gracz nacisnal klawisz Escape w celu zamkniecia strony.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CleanUpBeforeAndDestroy();
        }
    }

    /// <summary>
    /// Inicjalizuje strone, ustawiajac tytul i tresc.
    /// </summary>
    /// <param name="title">Tytul strony.</param>
    /// <param name="content">Tresc strony.</param>
    public void Init(string title, string content)
    {
        title_TMP.text = title;
        content_TMP.text = content;
    }

    /// <summary>
    /// Zamyka strone, wywolujac odpowiednie zdarzenia i niszczac obiekt.
    /// </summary>
    private void CleanUpBeforeAndDestroy()
    {
        GlobalEvents.FireOnStoppingReadingPage(this);
        GlobalEvents.FireOnAnyUIClose(this);

        Destroy(this.gameObject);
    }
}
