using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionUI : MonoBehaviour
{
    private const float TRANSITION_TIME = 0.2f;

    [SerializeField] private Image black_background_image;

    private void OnEnable()
    {
        GlobalEvents.OnStartingTransition += StartTransition;
    }

    private void OnDisable()
    {
        GlobalEvents.OnStartingTransition -= StartTransition;
    }

    private void StartTransition(object sender, GlobalEvents.OnStartingTransitionEventArgs args)
    {
        StartCoroutine(BeginTransition(args.time_after_the_transition_ends));
    }

    private IEnumerator BeginTransition(float wait_time_after_finishing)
    {
        float t = 0.0f;

        while(t < TRANSITION_TIME)
        {
            black_background_image.color = new(black_background_image.color.r,
                                               black_background_image.color.g,
                                               black_background_image.color.b,
                                               Mathf.Lerp(0.0f, 1.0f, t / TRANSITION_TIME));

            t += Time.deltaTime;

            yield return null;
        }

        black_background_image.color = new(black_background_image.color.r,
                                               black_background_image.color.g,
                                               black_background_image.color.b,
                                               1.0f);

        yield return new WaitForSeconds(wait_time_after_finishing);

        StartCoroutine(EndTransition());
    }

    private IEnumerator EndTransition()
    {
        float t = 0.0f;

        while (t < TRANSITION_TIME)
        {
            black_background_image.color = new(black_background_image.color.r,
                                               black_background_image.color.g,
                                               black_background_image.color.b,
                                               Mathf.Lerp(1.0f, 0.0f, t / TRANSITION_TIME));

            t += Time.deltaTime;

            yield return null;
        }

        black_background_image.color = new(black_background_image.color.r,
                                               black_background_image.color.g,
                                               black_background_image.color.b,
                                               0.0f);
    }
}
