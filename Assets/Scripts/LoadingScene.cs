using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    /// <summary>
    /// Zmienna kontroluj¹ca, czy to jest pierwsza klatka w aktualnej aktualizacji.
    /// </summary>
    private bool is_first_update = true;


    /// <summary>
    /// Metoda Update() jest wywo³ywana co klatkê w grze.
    /// </summary>
    private void Update()
    {
        // Sprawdzamy, czy to jest pierwsza klatka w tej aktualizacji
        if (is_first_update)
        {
            // Je¿eli to pierwsza klatka, ustawiamy flagê na false, aby unikn¹æ kolejnego wywo³ania w tej samej klatce
            is_first_update = false;
        }
        else
        {
            // Po pierwszej klatce, wywo³ujemy metodê LoaderCallback z klasy SceneController
            SceneController.LoaderCallback();

            // Po wywo³aniu callbacku, ustawiamy flagê is_first_update na true, aby powtórzyæ operacjê w nastêpnej klatce
            is_first_update = true;
        }
    }
}
