using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// Subskrybuje zdarzenia pauzy i wznowienia gry podczas aktywacji obiektu.
    /// </summary>
    private void OnEnable()
    {
        // Subskrypcja do globalnych zdarzen (pauza, wznowienie)
        GlobalEvents.OnPauseGame += PauseGame;
        GlobalEvents.OnResumeGame += ResumeGame;

        // Ustawienie czasu gry na normaln¹ predkosc (1.0f)
        Time.timeScale = 1.0f;
    }

    /// <summary>
    /// Anuluje subskrypcje zdarzen podczas dezaktywacji obiektu.
    /// </summary>
    private void OnDisable()
    {
        // Anulowanie subskrypcji globalnych zdarzen
        GlobalEvents.OnPauseGame -= PauseGame;
        GlobalEvents.OnResumeGame -= ResumeGame;
    }

    /// <summary>
    /// Wstrzymuje gre (czas gry = 0).
    /// </summary>
    private void PauseGame(object sender, EventArgs e)
    {
        Time.timeScale = 0f;  // Zatrzymuje czas w grze
    }

    /// <summary>
    /// Wznawia gre (czas gry = 1).
    /// </summary>
    private void ResumeGame(object sender, EventArgs e)
    {
        Time.timeScale = 1f;  // Przywraca normalny czas w grze
    }
}
