using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverProc : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SceneController.LoadScene(SceneController.GAME_OVER_SCENE);
    }
}
