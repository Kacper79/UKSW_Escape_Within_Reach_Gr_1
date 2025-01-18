using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za wykrywanie obiektów interaktywnych w zasiêgu gracza,
/// wyœwietlanie podpowiedzi interfejsu u¿ytkownika oraz obs³ugê interakcji.
/// </summary>
public class InteractableTargetsDetector : MonoBehaviour
{
    /// <summary>
    /// Maksymalny dystans, na którym mo¿na wykrywaæ obiekty interaktywne.
    /// </summary>
    private const float LOOK_FOR_INTERACTABLES_MAX_DISTANCE = 3.0f;

    /// <summary>
    /// Czas po uruchomieniu, po którym rozpoczyna siê wyszukiwanie obiektów interaktywnych.
    /// </summary>
    private const float TIME_IN_SECONDS_AFTER_LOOKING_FOR_INTERACTABLES_START = 0.3f;

    /// <summary>
    /// Czêstotliwoœæ w sekundach, z jak¹ powtarzane jest wyszukiwanie obiektów interaktywnych.
    /// </summary>
    private const float LOOKING_FOR_INTERACTABLES_REPEAT_RATE_IN_SECONDS = 0.1f;

    /// <summary>
    /// Punkt pocz¹tkowy raycastu wykrywaj¹cego obiekty interaktywne.
    /// </summary>
    [SerializeField] private Transform detector_raycast_origin;

    /// <summary>
    /// Kierunek raycastu wykrywaj¹cego obiekty interaktywne.
    /// </summary>
    [SerializeField] private Transform detector_raycast_direction;

    /// <summary>
    /// UI odpowiedzialny za wyœwietlanie podpowiedzi do interakcji.
    /// </summary>
    [SerializeField] private InteractTooltipUI interact_tooltip_UI;

    /// <summary>
    /// Pozycja pocz¹tkowa raycastu.
    /// </summary>
    private Vector3 ray_origin;

    /// <summary>
    /// Kierunek raycastu.
    /// </summary>
    private Vector3 ray_direction;

    /// <summary>
    /// Subskrybuje odpowiednie zdarzenia globalne i uruchamia proces wyszukiwania interaktywnych obiektów.
    /// </summary>
    private void OnEnable()
    {
        InvokeRepeating(nameof(LookForInteractableToShowUITooltip), TIME_IN_SECONDS_AFTER_LOOKING_FOR_INTERACTABLES_START, LOOKING_FOR_INTERACTABLES_REPEAT_RATE_IN_SECONDS);

        GlobalEvents.OnReadingPage += StopLookingForInteractable;
        GlobalEvents.OnStoppingReadingPage += StartLookingForInteractable;

        GlobalEvents.OnStartingDialogue += StopLookingForInteractable;
        GlobalEvents.OnEndingDialogue += StartLookingForInteractable;

        GlobalEvents.OnStartingBlackJackGameForMoney += StopLookingForInteractable;
        GlobalEvents.OnStartingBlackJackGameForPickaxe += StopLookingForInteractable;

        GlobalEvents.OnEndingBlackjackGame += StartLookingForInteractable;
    }

    /// <summary>
    /// Wyrejestrowuje zdarzenia globalne i zatrzymuje wyszukiwanie interaktywnych obiektów.
    /// </summary>
    private void OnDisable()
    {
        CancelInvoke(nameof(LookForInteractableToShowUITooltip));

        GlobalEvents.OnReadingPage -= StopLookingForInteractable;
        GlobalEvents.OnStoppingReadingPage -= StartLookingForInteractable;

        GlobalEvents.OnStartingDialogue -= StopLookingForInteractable;
        GlobalEvents.OnEndingDialogue -= StartLookingForInteractable;

        GlobalEvents.OnStartingBlackJackGameForMoney -= StopLookingForInteractable;
        GlobalEvents.OnStartingBlackJackGameForPickaxe -= StopLookingForInteractable;

        GlobalEvents.OnEndingBlackjackGame -= StartLookingForInteractable;
    }

    /// <summary>
    /// Zatrzymuje proces wyszukiwania interaktywnych obiektów po otrzymaniu odpowiedniego zdarzenia.
    /// </summary>
    private void StopLookingForInteractable(object sender, EventArgs e)
    {
        CancelInvoke(nameof(LookForInteractableToShowUITooltip));
    }

    /// <summary>
    /// Wznawia proces wyszukiwania interaktywnych obiektów po otrzymaniu odpowiedniego zdarzenia.
    /// </summary>
    private void StartLookingForInteractable(object sender, EventArgs e)
    {
        InvokeRepeating(nameof(LookForInteractableToShowUITooltip), TIME_IN_SECONDS_AFTER_LOOKING_FOR_INTERACTABLES_START, LOOKING_FOR_INTERACTABLES_REPEAT_RATE_IN_SECONDS);
    }

    /// <summary>
    /// Wykonuje raycast w celu wykrycia obiektów interaktywnych i wyœwietlenia ich podpowiedzi UI.
    /// </summary>
    private void LookForInteractableToShowUITooltip()
    {
        ray_origin = detector_raycast_origin.position;
        ray_direction = detector_raycast_direction.position - detector_raycast_origin.position;

        RaycastHit[] all_hits = Physics.RaycastAll(ray_origin, ray_direction, LOOK_FOR_INTERACTABLES_MAX_DISTANCE);

        foreach (RaycastHit hit in all_hits)
        {
            if (hit.collider != null && hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interact_tooltip_UI.SetTooltip(interactable.GetInteractionTooltip());
                interactable.AdditionalStuffWhenLookingAtInteractable();

                return;
            }
        }

        GlobalEvents.FireOnNotLookingOnInteractable(this);
    }

    /// <summary>
    /// Próbuje wykonaæ interakcjê z najbli¿szym wykrytym obiektem interaktywnym.
    /// </summary>
    public void TryInteracting()
    {
        ray_origin = detector_raycast_origin.position;
        ray_direction = detector_raycast_direction.position - detector_raycast_origin.position;

        RaycastHit[] all_hits = Physics.RaycastAll(ray_origin, ray_direction, LOOK_FOR_INTERACTABLES_MAX_DISTANCE);

        foreach (RaycastHit hit in all_hits)
        {
            if (hit.collider != null && hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact();

                return;
            }
        }
    }
}

