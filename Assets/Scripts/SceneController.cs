using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public const string GAME_SCENE = "Janek";
    public const string MAIN_MENU_SCENE = "MainMenuScene";
    public const string LOADING_SCENE = "LoadingScene";
    private static string target_scene;

    public static void LoadScene(string scene)
    {
        target_scene = scene;
        SceneManager.LoadScene(LOADING_SCENE);
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(target_scene);
    }
}
