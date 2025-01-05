using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : Singleton<UIFade>
{
    [SerializeField] private Image fadeScreen; // Assign the black screen image here
    [SerializeField] private float fadeSpeed = 1f;

    private IEnumerator fadeRoutine;

    protected override void Awake()
    {
        base.Awake();

        // Start with the fade screen fully transparent
        if (fadeScreen != null)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 0f);
        }
    }

    public void FadeToBlack()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(1f); // Fade to full black (alpha = 1)
        StartCoroutine(fadeRoutine);
    }

    public void FadeToClear()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(0f); // Fade to clear (alpha = 0)
        StartCoroutine(fadeRoutine);
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        if (fadeScreen == null) yield break;

        // Gradually adjust the alpha of the fade screen
        while (!Mathf.Approximately(fadeScreen.color.a, targetAlpha))
        {
            float newAlpha = Mathf.MoveTowards(fadeScreen.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, newAlpha);
            yield return null;
        }
    }
}
