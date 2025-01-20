using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za wyswietlanie tekstu, ktory jest wypowiadany przez NPC w grze.
/// </summary>
public class SpokenTextDisplayUI : MonoBehaviour
{
    /// <summary>
    /// Komponent TextMeshProUGUI do wyswietlania tekstu wypowiadanego przez NPC.
    /// </summary>
    [SerializeField] private TextMeshProUGUI spoken_text;

    /// <summary>
    /// Inicjalizuje komponenty UI i ustawia je w stanie poczatkowym (wszystkie komponenty sa wylaczone).
    /// </summary>
    private void Awake()
    {
        DisableAllComponents();
    }

    /// <summary>
    /// Wyswietla tekst wypowiadany przez NPC na ekranie.
    /// </summary>
    /// <param name="text">Tekst wypowiadany przez NPC.</param>
    /// <param name="npc_name">Imie NPC wypowiadajacego tekst.</param>
    public void DisplayText(string text, string npc_name)
    {
        EnableAllComponents();
        spoken_text.text = $"{npc_name}: {text}";
    }

    /// <summary>
    /// Wlacza wszystkie komponenty wyswietlania tekstu.
    /// </summary>
    private void EnableAllComponents()
    {
        spoken_text.gameObject.SetActive(true);
    }

    /// <summary>
    /// Wylacza wszystkie komponenty wyswietlania tekstu.
    /// </summary>
    public void DisableAllComponents()
    {
        spoken_text.gameObject.SetActive(false);
    }
}
