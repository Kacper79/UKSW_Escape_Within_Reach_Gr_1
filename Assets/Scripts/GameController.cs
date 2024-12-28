using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    private void OnEnable()
    {
        GlobalEvents.OnPauseGame += PauseGame;
        GlobalEvents.OnResumeGame += ResumeGame;

        Time.timeScale = 1.0f;
    }

    private void OnDisable()
    {
        GlobalEvents.OnPauseGame -= PauseGame;
        GlobalEvents.OnResumeGame -= ResumeGame;
    }

    private void PauseGame(object sender, EventArgs e)
    {
        Time.timeScale = 0f;
    }

    private void ResumeGame(object sender, EventArgs e)
    {
        Time.timeScale = 1f;
    }
}
