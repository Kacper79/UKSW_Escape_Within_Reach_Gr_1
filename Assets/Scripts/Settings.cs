using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    /// <summary>
    /// Indeks ustawionej rozdzielczoœci w liœcie dostêpnych rozdzielczoœci.
    /// </summary>
    private static int resolution_index;

    /// <summary>
    /// Szerokoœæ ustawionej rozdzielczoœci.
    /// </summary>
    private static int resolution_width;

    /// <summary>
    /// Wysokoœæ ustawionej rozdzielczoœci.
    /// </summary>
    private static int resolution_height;

    /// <summary>
    /// Ustawienia g³oœnoœci muzyki.
    /// </summary>
    private static float music_volume;

    /// <summary>
    /// Ustawienia g³oœnoœci efektów dŸwiêkowych.
    /// </summary>
    private static float audio_effects_volume;

    /// <summary>
    /// Ustawienie czu³oœci myszy.
    /// </summary>
    private static float mouse_sensitivity;

    /// <summary>
    /// Statyczny konstruktor klasy, ustawia domyœlne wartoœci ustawieñ.
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
    /// Ustawia parametry rozdzielczoœci na podstawie tekstu z dropdowna.
    /// </summary>
    /// <param name="dropdown_resolution_text">Tekst rozdzielczoœci w formacie "width x height"</param>
    public static void SetResolutionParameters(string dropdown_resolution_text)
    {
        dropdown_resolution_text = dropdown_resolution_text.Replace(" ", ""); // Usuwa spacje

        string[] resolution_parts = dropdown_resolution_text.Split('x'); // Dzieli tekst na szerokoœæ i wysokoœæ

        resolution_width = int.Parse(resolution_parts[0]); // Ustawia szerokoœæ
        resolution_height = int.Parse(resolution_parts[1]); // Ustawia wysokoœæ
    }

    /// <summary>
    /// Ustawia indeks rozdzielczoœci
    /// </summary>
    /// <param name="index">Indeks rozdzielczoœci w systemie</param>
    public static void SetResolutionIndex(int index)
    {
        resolution_index = index;
    }

    /// <summary>
    /// Zwraca indeks aktualnej rozdzielczoœci
    /// </summary>
    /// <returns>Indeks rozdzielczoœci</returns>
    public static int GetResolutionIndex()
    {
        return resolution_index;
    }

    /// <summary>
    /// Zwraca szerokoœæ aktualnej rozdzielczoœci
    /// </summary>
    /// <returns>Szerokoœæ rozdzielczoœci</returns>
    public static int GetResolutionWidth()
    {
        return resolution_width;
    }

    /// <summary>
    /// Zwraca wysokoœæ aktualnej rozdzielczoœci
    /// </summary>
    /// <returns>Wysokoœæ rozdzielczoœci</returns>
    public static int GetResolutionHeight()
    {
        return resolution_height;
    }

    /// <summary>
    /// Ustawia g³oœnoœæ muzyki
    /// </summary>
    /// <param name="value">Poziom g³oœnoœci (0 do 1)</param>
    public static void SetMusicVolume(float value)
    {
        music_volume = value;
    }

    /// <summary>
    /// Zwraca aktualn¹ g³oœnoœæ muzyki
    /// </summary>
    /// <returns>G³oœnoœæ muzyki</returns>
    public static float GetMusicVolume()
    {
        return music_volume;
    }

    /// <summary>
    /// Ustawia g³oœnoœæ efektów dŸwiêkowych
    /// </summary>
    /// <param name="value">Poziom g³oœnoœci efektów (0 do 1)</param>
    public static void SetAudioEffectsVolume(float value)
    {
        audio_effects_volume = value;
    }

    /// <summary>
    /// Zwraca aktualn¹ g³oœnoœæ efektów dŸwiêkowych
    /// </summary>
    /// <returns>G³oœnoœæ efektów dŸwiêkowych</returns>
    public static float GetAudioEffectsVolume()
    {
        return audio_effects_volume;
    }

    /// <summary>
    /// Ustawia czu³oœæ myszy
    /// </summary>
    /// <param name="sens">Nowa czu³oœæ myszy</param>
    public static void SetSensitivity(float sens)
    {
        mouse_sensitivity = sens;
    }

    /// <summary>
    /// Zwraca aktualn¹ czu³oœæ myszy
    /// </summary>
    /// <returns>Czu³oœæ myszy</returns>
    public static float GetSensitivity()
    {
        return mouse_sensitivity;
    }
}
