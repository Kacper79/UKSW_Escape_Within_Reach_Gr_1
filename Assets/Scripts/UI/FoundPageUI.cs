using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoundPageUI : MonoBehaviour
{
    [Header("Content references")]
    [SerializeField] private TextMeshProUGUI title_TMP;
    [SerializeField] private TextMeshProUGUI content_TMP;

    private int times_interaction_button_was_pressed = 0;//it registered E press from Interaction and instantly destroyed the page, so had to implement some system, hopefully this will do

    private void Start()
    {
        GlobalEvents.FireOnAnyUIOpen(this);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CleanUpBeforeAndDestroy();
        }
    }

    public void Init(string title, string content)
    {
        title_TMP.text = title;
        content_TMP.text = content;
    }

    private void CleanUpBeforeAndDestroy()
    {
        GlobalEvents.FireOnStoppingReadingPage(this);
        GlobalEvents.FireOnAnyUIClose(this);

        Destroy(this.gameObject);
    }
}
