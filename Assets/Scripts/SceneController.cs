using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    /// <summary>
    /// Stale reprezentujace nazwy scen w grze.
    /// </summary>
    public const string GAME_SCENE = "Kacper2";
    public const string MAIN_MENU_SCENE = "MainMenuScene";
    public const string LOADING_SCENE = "LoadingScene";
    public const string GAME_OVER_SCENE = "GameOver";

    /// <summary>
    /// Przechowuje nazwe docelowej sceny.
    /// </summary>
    private static string target_scene;


    /// <summary>
    /// Laduje scene gry przy uzyciu sceny ladowania jako posrednika
    /// </summary>
    /// <param name="scene">Nazwa sceny do zaladowania</param>
    public static void LoadScene(string scene)
    {
        target_scene = scene;  // Ustawia scene docelowa
        SceneManager.LoadScene(LOADING_SCENE);  // Laduje scene ladowania
    }

    /// <summary>
    /// Callback, ktory jest wywolywany po zaladowaniu sceny ladowania.
    /// Przeladowuje docelowa scene.
    /// </summary>
    public static void LoaderCallback()
    {
        SceneManager.LoadScene(target_scene);  // Laduje docelowa scene
    }
}
