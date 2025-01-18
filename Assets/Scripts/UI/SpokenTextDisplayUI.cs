using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za wyœwietlanie tekstu, który jest wypowiadany przez NPC w grze.
/// </summary>
public class SpokenTextDisplayUI : MonoBehaviour
{
    /// <summary>
    /// Komponent TextMeshProUGUI do wyœwietlania tekstu wypowiadanego przez NPC.
    /// </summary>
    [SerializeField] private TextMeshProUGUI spoken_text;

    /// <summary>
    /// Inicjalizuje komponenty UI i ustawia je w stanie pocz¹tkowym (wszystkie komponenty s¹ wy³¹czone).
    /// </summary>
    private void Awake()
    {
        DisableAllComponents();
    }

    /// <summary>
    /// Wyœwietla tekst wypowiadany przez NPC na ekranie.
    /// </summary>
    /// <param name="text">Tekst wypowiadany przez NPC.</param>
    /// <param name="npc_name">Imiê NPC wypowiadaj¹cego tekst.</param>
    public void DisplayText(string text, string npc_name)
    {
        EnableAllComponents();
        spoken_text.text = $"{npc_name}: {text}";
    }

    /// <summary>
    /// W³¹cza wszystkie komponenty wyœwietlania tekstu.
    /// </summary>
    private void EnableAllComponents()
    {
        spoken_text.gameObject.SetActive(true);
    }

    /// <summary>
    /// Wy³¹cza wszystkie komponenty wyœwietlania tekstu.
    /// </summary>
    public void DisableAllComponents()
    {
        spoken_text.gameObject.SetActive(false);
    }
}
