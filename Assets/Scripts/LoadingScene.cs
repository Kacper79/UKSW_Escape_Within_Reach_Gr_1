using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    private bool is_first_update = true;

    private void Update()
    {
        if (is_first_update)
        {
            is_first_update = false;
        }
        else
        {
            SceneController.LoaderCallback();
            is_first_update = true;
        }
    }
}
