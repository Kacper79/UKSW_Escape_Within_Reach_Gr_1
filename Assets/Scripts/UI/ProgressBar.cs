using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image background_bar;  // T�o paska post�pu
    [SerializeField] private Image progress_bar;   // W�a�ciwy pasek post�pu

    /// <summary>
    /// Ustawia warto�ci paska post�pu na podstawie bie��cego post�pu i maksymalnego post�pu.
    /// Zmienia szeroko�� paska post�pu w zale�no�ci od warto�ci.
    /// </summary>
    /// <param name="progress">Aktualny post�p (np. poziom stresu lub HP).</param>
    /// <param name="max_progress">Maksymalny post�p, kt�ry mo�e zosta� osi�gni�ty (np. maksymalne HP lub maksymalny poziom stresu).</param>
    public void SetProgressBarValues(float progress, float max_progress)
    {
        // Oblicza stosunek post�pu do maksymalnego post�pu
        float progressRatio = progress / max_progress;

        // Ustawia szeroko�� paska post�pu na podstawie obliczonego stosunku
        progress_bar.rectTransform.sizeDelta = new(progressRatio * background_bar.rectTransform.rect.width, progress_bar.rectTransform.sizeDelta.y);
    }
}
