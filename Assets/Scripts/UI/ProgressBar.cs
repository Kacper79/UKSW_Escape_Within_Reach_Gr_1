using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image background_bar;  // T³o paska postêpu
    [SerializeField] private Image progress_bar;   // W³aœciwy pasek postêpu

    /// <summary>
    /// Ustawia wartoœci paska postêpu na podstawie bie¿¹cego postêpu i maksymalnego postêpu.
    /// Zmienia szerokoœæ paska postêpu w zale¿noœci od wartoœci.
    /// </summary>
    /// <param name="progress">Aktualny postêp (np. poziom stresu lub HP).</param>
    /// <param name="max_progress">Maksymalny postêp, który mo¿e zostaæ osi¹gniêty (np. maksymalne HP lub maksymalny poziom stresu).</param>
    public void SetProgressBarValues(float progress, float max_progress)
    {
        // Oblicza stosunek postêpu do maksymalnego postêpu
        float progressRatio = progress / max_progress;

        // Ustawia szerokoœæ paska postêpu na podstawie obliczonego stosunku
        progress_bar.rectTransform.sizeDelta = new(progressRatio * background_bar.rectTransform.rect.width, progress_bar.rectTransform.sizeDelta.y);
    }
}
