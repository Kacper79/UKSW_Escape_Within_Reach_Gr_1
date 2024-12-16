using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpokenTextDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI spoken_text;

    private void Awake()
    {
        DisableAllComponents();
    }

    public void DisplayText(string text, string npc_name)
    {
        EnableAllComponents();
        spoken_text.text = $"{npc_name}: {text}";
    }

    private void EnableAllComponents()
    {
        spoken_text.gameObject.SetActive(true);
    }

    public void DisableAllComponents()
    {
        spoken_text.gameObject.SetActive(false);
    }
}
