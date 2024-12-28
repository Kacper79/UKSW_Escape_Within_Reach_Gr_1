using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    private static int resolution_index;
    private static int resolution_width;
    private static int resolution_height;

    private static float music_volume;
    private static float audio_effects_volume;
    private static float mouse_sensitivity;

    static Settings()
    {
        resolution_index = 5;
        resolution_width = 1920;
        resolution_height = 1080;

        music_volume = 1f;
        audio_effects_volume = 1f;

        mouse_sensitivity = 10f;
    }
    public static void SetResolutionParameters(string dropdown_resolution_text)
    {
        dropdown_resolution_text = dropdown_resolution_text.Replace(" ", "");

        string[] resolution_parts = dropdown_resolution_text.Split('x');

        resolution_width = int.Parse(resolution_parts[0]);
        resolution_height = int.Parse(resolution_parts[1]);
    }
    public static void SetResolutionIndex(int index)
    {
        resolution_index = index;
    }

    public static int GetResolutionIndex()
    {
        return resolution_index;
    }

    public static int GetResolutionWidth()
    {
        return resolution_width;
    }

    public static int GetResolutionHeight()
    {
        return resolution_height;
    }

    public static void SetMusicVolume(float value)
    {
        music_volume = value;
    }

    public static float GetMusicVolume()
    {
        return music_volume;
    }

    public static void SetAudioEffectsVolume(float value)
    {
        audio_effects_volume = value;
    }

    public static float GetAudioEffectsVolume()
    {
        return audio_effects_volume;
    }
    public static void SetSensitivity(float sens)
    {
        mouse_sensitivity = sens;
    }

    public static float GetSensitivity()
    {
        return mouse_sensitivity;
    }
}
