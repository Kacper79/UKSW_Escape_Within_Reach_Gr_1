using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button exit_button;

    private void Awake()
    {
        exit_button.onClick.AddListener(Exit);
    }

    private void Exit()
    {
        Application.Quit();
    }
}
