using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    /// <summary>
    /// Indeks ustawionej rozdzielczosci w liœcie dostepnych rozdzielczosci.
    /// </summary>
    private static int resolution_index;

    /// <summary>
    /// Szerokosc ustawionej rozdzielczosci.
    /// </summary>
    private static int resolution_width;

    /// <summary>
    /// Wysokosc ustawionej rozdzielczosci.
    /// </summary>
    private static int resolution_height;

    /// <summary>
    /// Ustawienia glosnosci muzyki.
    /// </summary>
    private static float music_volume;

    /// <summary>
    /// Ustawienia glosnosci efektow dzwiekowych.
    /// </summary>wysokosc
    private static float audio_effects_volume;

    /// <summary>
    /// Ustawienie czulosci myszy.
    /// </summary>
    private static float mouse_sensitivity;

    /// <summary>
    /// Statyczny konstruktor klasy, ustawia domyslne wartosci ustawien.
    /// </summary>
    static Settings()
    {
        resolution_index = 5;
        resolution_width = 1920;
        resolution_height = 1080;

        music_volume = 1f;
        audio_effects_volume = 1f;

        mouse_sensitivity = 10f;
    }


    /// <summary>
    /// Ustawia parametry rozdzielczosci na podstawie tekstu z dropdowna.
    /// </summary>
    /// <param name="dropdown_resolution_text">Tekst rozdzielczosci w formacie "width x height"</param>
    public static void SetResolutionParameters(string dropdown_resolution_text)
    {
        dropdown_resolution_text = dropdown_resolution_text.Replace(" ", ""); // Usuwa spacje

        string[] resolution_parts = dropdown_resolution_text.Split('x'); // Dzieli tekst na szerokosc i wysokosc

        resolution_width = int.Parse(resolution_parts[0]); // Ustawia szerokosc
        resolution_height = int.Parse(resolution_parts[1]); // Ustawia wysokosc
    }

    /// <summary>
    /// Ustawia indeks rozdzielczosci
    /// </summary>
    /// <param name="index">Indeks rozdzielczosci w systemie</param>
    public static void SetResolutionIndex(int index)
    {
        resolution_index = index;
    }

    /// <summary>
    /// Zwraca indeks aktualnej rozdzielczosci
    /// </summary>
    /// <returns>Indeks rozdzielczosci</returns>
    public static int GetResolutionIndex()
    {
        return resolution_index;
    }

    /// <summary>
    /// Zwraca szerokosc aktualnej rozdzielczosci
    /// </summary>
    /// <returns>Szerokosc rozdzielczosci</returns>
    public static int GetResolutionWidth()
    {
        return resolution_width;
    }

    /// <summary>
    /// Zwraca wysokosc aktualnej rozdzielczosci
    /// </summary>
    /// <returns>Wysokosc rozdzielczosci</returns>
    public static int GetResolutionHeight()
    {
        return resolution_height;
    }

    /// <summary>
    /// Ustawia glosnosc muzyki
    /// </summary>
    /// <param name="value">Poziom glosnosci (0 do 1)</param>
    public static void SetMusicVolume(float value)
    {
        music_volume = value;
        AudioManager.Instance.ChangeMusicVolume(value);
    }

    /// <summary>
    /// Zwraca aktualna glosnosc muzyki
    /// </summary>
    /// <returns>G³oœnosc muzyki</returns>
    public static float GetMusicVolume()
    {
        return music_volume;
    }

    /// <summary>
    /// Ustawia glosnosc efektow dzwiekowych
    /// </summary>
    /// <param name="value">Poziom glosnosci efektow (0 do 1)</param>
    public static void SetAudioEffectsVolume(float value)
    {
        audio_effects_volume = value;
    }

    /// <summary>
    /// Zwraca aktualna glosnosc efektow dzwiekowych
    /// </summary>
    /// <returns>G³oœnosc efektow dzwiekowych</returns>
    public static float GetAudioEffectsVolume()
    {
        return audio_effects_volume;
    }

    /// <summary>
    /// Ustawia czulosc myszy
    /// </summary>
    /// <param name="sens">Nowa czulosc myszy</param>
    public static void SetSensitivity(float sens)
    {
        mouse_sensitivity = sens;
    }

    /// <summary>
    /// Zwraca aktualna czulosc myszy
    /// </summary>
    /// <returns>Czulosc myszy</returns>
    public static float GetSensitivity()
    {
        return mouse_sensitivity;
    }
}
