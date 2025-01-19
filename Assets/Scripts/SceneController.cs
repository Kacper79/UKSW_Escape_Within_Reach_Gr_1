using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    /// <summary>
    /// Sta³e reprezentuj¹ce nazwy scen w grze.
    /// </summary>
    public const string GAME_SCENE = "Kacper";
    public const string MAIN_MENU_SCENE = "MainMenuScene";
    public const string LOADING_SCENE = "LoadingScene";

    /// <summary>
    /// Przechowuje nazwê docelowej sceny.
    /// </summary>
    private static string target_scene;


    /// <summary>
    /// £aduje scenê gry przy u¿yciu sceny ³adowania jako poœrednika
    /// </summary>
    /// <param name="scene">Nazwa sceny do za³adowania</param>
    public static void LoadScene(string scene)
    {
        target_scene = scene;  // Ustawia scenê docelow¹
        SceneManager.LoadScene(LOADING_SCENE);  // £aduje scenê ³adowania
    }

    /// <summary>
    /// Callback, który jest wywo³ywany po za³adowaniu sceny ³adowania.
    /// Prze³adowuje docelow¹ scenê.
    /// </summary>
    public static void LoaderCallback()
    {
        SceneManager.LoadScene(target_scene);  // £aduje docelow¹ scenê
    }
}
