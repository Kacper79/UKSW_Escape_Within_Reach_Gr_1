using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    /// <summary>
    /// Indeks ustawionej rozdzielczo�ci w li�cie dost�pnych rozdzielczo�ci.
    /// </summary>
    private static int resolution_index;

    /// <summary>
    /// Szeroko�� ustawionej rozdzielczo�ci.
    /// </summary>
    private static int resolution_width;

    /// <summary>
    /// Wysoko�� ustawionej rozdzielczo�ci.
    /// </summary>
    private static int resolution_height;

    /// <summary>
    /// Ustawienia g�o�no�ci muzyki.
    /// </summary>
    private static float music_volume;

    /// <summary>
    /// Ustawienia g�o�no�ci efekt�w d�wi�kowych.
    /// </summary>
    private static float audio_effects_volume;

    /// <summary>
    /// Ustawienie czu�o�ci myszy.
    /// </summary>
    private static float mouse_sensitivity;

    /// <summary>
    /// Statyczny konstruktor klasy, ustawia domy�lne warto�ci ustawie�.
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
    /// Ustawia parametry rozdzielczo�ci na podstawie tekstu z dropdowna.
    /// </summary>
    /// <param name="dropdown_resolution_text">Tekst rozdzielczo�ci w formacie "width x height"</param>
    public static void SetResolutionParameters(string dropdown_resolution_text)
    {
        dropdown_resolution_text = dropdown_resolution_text.Replace(" ", ""); // Usuwa spacje

        string[] resolution_parts = dropdown_resolution_text.Split('x'); // Dzieli tekst na szeroko�� i wysoko��

        resolution_width = int.Parse(resolution_parts[0]); // Ustawia szeroko��
        resolution_height = int.Parse(resolution_parts[1]); // Ustawia wysoko��
    }

    /// <summary>
    /// Ustawia indeks rozdzielczo�ci
    /// </summary>
    /// <param name="index">Indeks rozdzielczo�ci w systemie</param>
    public static void SetResolutionIndex(int index)
    {
        resolution_index = index;
    }

    /// <summary>
    /// Zwraca indeks aktualnej rozdzielczo�ci
    /// </summary>
    /// <returns>Indeks rozdzielczo�ci</returns>
    public static int GetResolutionIndex()
    {
        return resolution_index;
    }

    /// <summary>
    /// Zwraca szeroko�� aktualnej rozdzielczo�ci
    /// </summary>
    /// <returns>Szeroko�� rozdzielczo�ci</returns>
    public static int GetResolutionWidth()
    {
        return resolution_width;
    }

    /// <summary>
    /// Zwraca wysoko�� aktualnej rozdzielczo�ci
    /// </summary>
    /// <returns>Wysoko�� rozdzielczo�ci</returns>
    public static int GetResolutionHeight()
    {
        return resolution_height;
    }

    /// <summary>
    /// Ustawia g�o�no�� muzyki
    /// </summary>
    /// <param name="value">Poziom g�o�no�ci (0 do 1)</param>
    public static void SetMusicVolume(float value)
    {
        music_volume = value;
    }

    /// <summary>
    /// Zwraca aktualn� g�o�no�� muzyki
    /// </summary>
    /// <returns>G�o�no�� muzyki</returns>
    public static float GetMusicVolume()
    {
        return music_volume;
    }

    /// <summary>
    /// Ustawia g�o�no�� efekt�w d�wi�kowych
    /// </summary>
    /// <param name="value">Poziom g�o�no�ci efekt�w (0 do 1)</param>
    public static void SetAudioEffectsVolume(float value)
    {
        audio_effects_volume = value;
    }

    /// <summary>
    /// Zwraca aktualn� g�o�no�� efekt�w d�wi�kowych
    /// </summary>
    /// <returns>G�o�no�� efekt�w d�wi�kowych</returns>
    public static float GetAudioEffectsVolume()
    {
        return audio_effects_volume;
    }

    /// <summary>
    /// Ustawia czu�o�� myszy
    /// </summary>
    /// <param name="sens">Nowa czu�o�� myszy</param>
    public static void SetSensitivity(float sens)
    {
        mouse_sensitivity = sens;
    }

    /// <summary>
    /// Zwraca aktualn� czu�o�� myszy
    /// </summary>
    /// <returns>Czu�o�� myszy</returns>
    public static float GetSensitivity()
    {
        return mouse_sensitivity;
    }
}
