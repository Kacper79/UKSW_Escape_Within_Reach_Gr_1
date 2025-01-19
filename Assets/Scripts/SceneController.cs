using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    /// <summary>
    /// Sta�e reprezentuj�ce nazwy scen w grze.
    /// </summary>
    public const string GAME_SCENE = "Kacper";
    public const string MAIN_MENU_SCENE = "MainMenuScene";
    public const string LOADING_SCENE = "LoadingScene";

    /// <summary>
    /// Przechowuje nazw� docelowej sceny.
    /// </summary>
    private static string target_scene;


    /// <summary>
    /// �aduje scen� gry przy u�yciu sceny �adowania jako po�rednika
    /// </summary>
    /// <param name="scene">Nazwa sceny do za�adowania</param>
    public static void LoadScene(string scene)
    {
        target_scene = scene;  // Ustawia scen� docelow�
        SceneManager.LoadScene(LOADING_SCENE);  // �aduje scen� �adowania
    }

    /// <summary>
    /// Callback, kt�ry jest wywo�ywany po za�adowaniu sceny �adowania.
    /// Prze�adowuje docelow� scen�.
    /// </summary>
    public static void LoaderCallback()
    {
        SceneManager.LoadScene(target_scene);  // �aduje docelow� scen�
    }
}
