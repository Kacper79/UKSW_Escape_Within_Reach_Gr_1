using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    /// <summary>
    /// Zmienna kontroluj�ca, czy to jest pierwsza klatka w aktualnej aktualizacji.
    /// </summary>
    private bool is_first_update = true;


    /// <summary>
    /// Metoda Update() jest wywo�ywana co klatk� w grze.
    /// </summary>
    private void Update()
    {
        // Sprawdzamy, czy to jest pierwsza klatka w tej aktualizacji
        if (is_first_update)
        {
            // Je�eli to pierwsza klatka, ustawiamy flag� na false, aby unikn�� kolejnego wywo�ania w tej samej klatce
            is_first_update = false;
        }
        else
        {
            // Po pierwszej klatce, wywo�ujemy metod� LoaderCallback z klasy SceneController
            SceneController.LoaderCallback();

            // Po wywo�aniu callbacku, ustawiamy flag� is_first_update na true, aby powt�rzy� operacj� w nast�pnej klatce
            is_first_update = true;
        }
    }
}
