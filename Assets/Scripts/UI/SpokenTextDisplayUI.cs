using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za wy�wietlanie tekstu, kt�ry jest wypowiadany przez NPC w grze.
/// </summary>
public class SpokenTextDisplayUI : MonoBehaviour
{
    /// <summary>
    /// Komponent TextMeshProUGUI do wy�wietlania tekstu wypowiadanego przez NPC.
    /// </summary>
    [SerializeField] private TextMeshProUGUI spoken_text;

    /// <summary>
    /// Inicjalizuje komponenty UI i ustawia je w stanie pocz�tkowym (wszystkie komponenty s� wy��czone).
    /// </summary>
    private void Awake()
    {
        DisableAllComponents();
    }

    /// <summary>
    /// Wy�wietla tekst wypowiadany przez NPC na ekranie.
    /// </summary>
    /// <param name="text">Tekst wypowiadany przez NPC.</param>
    /// <param name="npc_name">Imi� NPC wypowiadaj�cego tekst.</param>
    public void DisplayText(string text, string npc_name)
    {
        EnableAllComponents();
        spoken_text.text = $"{npc_name}: {text}";
    }

    /// <summary>
    /// W��cza wszystkie komponenty wy�wietlania tekstu.
    /// </summary>
    private void EnableAllComponents()
    {
        spoken_text.gameObject.SetActive(true);
    }

    /// <summary>
    /// Wy��cza wszystkie komponenty wy�wietlania tekstu.
    /// </summary>
    public void DisableAllComponents()
    {
        spoken_text.gameObject.SetActive(false);
    }
}
