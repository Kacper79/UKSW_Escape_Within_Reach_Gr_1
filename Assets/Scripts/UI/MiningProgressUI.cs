using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningProgressUI : MonoBehaviour
{
    [SerializeField] private MiningProgressBar mining_progress_bar;

    private void Start()
    {
        DisableUI();
    }

    private void OnEnable()
    {
        GlobalEvents.OnNotLookingOnInteractable += DisableUI;
    }

    private void OnDisable()
    {
        GlobalEvents.OnNotLookingOnInteractable -= DisableUI;
    }

    private void DisableUI(object sender, System.EventArgs e)
    {
        DisableUI();
    }

    public void EnableUI()
    {
        mining_progress_bar.gameObject.SetActive(true);
    }

    public void DisableUI()
    {
        mining_progress_bar.gameObject.SetActive(false);
    }

    public void PassValuesToProgressbar(float progress, float max_progress)
    {
        mining_progress_bar.SetProgressBarValues(progress, max_progress);
    }
}
