using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image background_bar;
    [SerializeField] private Image progress_bar;

    public void SetProgressBarValues(float progress, float max_progress)
    {
        float progressRatio = progress / max_progress;

        progress_bar.rectTransform.sizeDelta = new(progressRatio * background_bar.rectTransform.rect.width, progress_bar.rectTransform.sizeDelta.y);
    }
}
