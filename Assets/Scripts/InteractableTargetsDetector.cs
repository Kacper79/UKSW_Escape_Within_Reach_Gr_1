using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za wykrywanie obiektow interaktywnych w zasiegu gracza,
/// wyswietlanie podpowiedzi interfejsu uzytkownika oraz obsluge interakcji.
/// </summary>
public class InteractableTargetsDetector : MonoBehaviour
{
    /// <summary>
    /// Maksymalny dystans, na ktorym mozna wykrywacobiekty interaktywne.
    /// </summary>
    private const float LOOK_FOR_INTERACTABLES_MAX_DISTANCE = 3.0f;

    /// <summary>
    /// Czas po uruchomieniu, po ktorym rozpoczyna sie wyszukiwanie obiektow interaktywnych.
    /// </summary>
    private const float TIME_IN_SECONDS_AFTER_LOOKING_FOR_INTERACTABLES_START = 0.3f;

    /// <summary>
    /// Czestotliwosc w sekundach, z jaka powtarzane jest wyszukiwanie obiektow interaktywnych.
    /// </summary>
    private const float LOOKING_FOR_INTERACTABLES_REPEAT_RATE_IN_SECONDS = 0.1f;

    /// <summary>
    /// Punkt poczatkowy raycastu wykrywajacego obiekty interaktywne.
    /// </summary>
    [SerializeField] private Transform detector_raycast_origin;

    /// <summary>
    /// Kierunek raycastu wykrywajacego obiekty interaktywne.
    /// </summary>
    [SerializeField] private Transform detector_raycast_direction;

    /// <summary>
    /// UI odpowiedzialny za wyswietlanie podpowiedzi do interakcji.
    /// </summary>
    [SerializeField] private InteractTooltipUI interact_tooltip_UI;

    /// <summary>
    /// Pozycja poczatkowa raycastu.
    /// </summary>
    private Vector3 ray_origin;

    /// <summary>
    /// Kierunek raycastu.
    /// </summary>
    private Vector3 ray_direction;

    /// <summary>
    /// Subskrybuje odpowiednie zdarzenia globalne i uruchamia proces wyszukiwania interaktywnych obiektow.
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
    /// Wyrejestrowuje zdarzenia globalne i zatrzymuje wyszukiwanie interaktywnych obiektow.
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
    /// Zatrzymuje proces wyszukiwania interaktywnych obiektow po otrzymaniu odpowiedniego zdarzenia.
    /// </summary>
    private void StopLookingForInteractable(object sender, EventArgs e)
    {
        CancelInvoke(nameof(LookForInteractableToShowUITooltip));
    }

    /// <summary>
    /// Wznawia proces wyszukiwania interaktywnych obiektow po otrzymaniu odpowiedniego zdarzenia.
    /// </summary>
    private void StartLookingForInteractable(object sender, EventArgs e)
    {
        InvokeRepeating(nameof(LookForInteractableToShowUITooltip), TIME_IN_SECONDS_AFTER_LOOKING_FOR_INTERACTABLES_START, LOOKING_FOR_INTERACTABLES_REPEAT_RATE_IN_SECONDS);
    }

    /// <summary>
    /// Wykonuje raycast w celu wykrycia obiektow interaktywnych i wyswietlenia ich podpowiedzi UI.
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
    /// Probuje wykonac interakcje z najblizszym wykrytym obiektem interaktywnym.
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

