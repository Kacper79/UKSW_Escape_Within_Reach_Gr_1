using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    /// <summary>
    /// Zmienna kontrolujaca, czy to jest pierwsza klatka w aktualnej aktualizacji.
    /// </summary>
    private bool is_first_update = true;


    /// <summary>
    /// Metoda Update() jest wywolywana co klatke w grze.
    /// </summary>
    private void Update()
    {
        // Sprawdzamy, czy to jest pierwsza klatka w tej aktualizacji
        if (is_first_update)
        {
            // Jeseli to pierwsza klatka, ustawiamy flage na false, aby uniknac kolejnego wywolania w tej samej klatce
            is_first_update = false;
        }
        else
        {
            // Po pierwszej klatce, wywolujemy metode LoaderCallback z klasy SceneController
            SceneController.LoaderCallback();

            // Po wywolaniu callbacku, ustawiamy flage is_first_update na true, aby powtorzyc operacje w nastepnej klatce
            is_first_update = true;
        }
    }
}
