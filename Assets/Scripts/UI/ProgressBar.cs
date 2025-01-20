using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image background_bar;  // tlo paska postepu
    [SerializeField] private Image progress_bar;   // Wlasciwy pasek postepu

    /// <summary>
    /// Ustawia wartosci paska postepu na podstawie biezacego postepu i maksymalnego postepu.
    /// Zmienia szerokosc paska postepu w zaleznosci od wartosci.
    /// </summary>
    /// <param name="progress">Aktualny postep (np. poziom stresu lub HP).</param>
    /// <param name="max_progress">Maksymalny postep, ktory moze zostaæ osi¹gniety (np. maksymalne HP lub maksymalny poziom stresu).</param>
    public void SetProgressBarValues(float progress, float max_progress)
    {
        // Oblicza stosunek postepu do maksymalnego postepu
        float progressRatio = progress / max_progress;

        // Ustawia szerokosc paska postepu na podstawie obliczonego stosunku
        progress_bar.rectTransform.sizeDelta = new(progressRatio * background_bar.rectTransform.rect.width, progress_bar.rectTransform.sizeDelta.y);
    }
}
