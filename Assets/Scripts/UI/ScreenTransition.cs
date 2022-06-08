using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] Image imageToFade;

    private float solidAlpha = 1f;
    private float clearAlpha = 0f;

    private void SetAlpha(float alpha)
    {
        if (imageToFade != null)
        {
            imageToFade.canvasRenderer.SetAlpha(alpha);
        }
    }

    private void Fade(float targetAlpha, float duration)
    {
        if (imageToFade != null)
        {
            imageToFade.gameObject.SetActive(true);
            if (imageToFade.color.a != targetAlpha)
            {
                imageToFade.CrossFadeAlpha(targetAlpha, duration, true);
            }
            else
            {
                imageToFade.gameObject.SetActive(false);
            }
        }
    }

    public void FadeOut(float fadeDuration)
    {
        SetAlpha(solidAlpha);
        Fade(clearAlpha, fadeDuration);
    }

    public void FadeIn(float fadeDuration)
    {
        SetAlpha(clearAlpha);
        Fade(solidAlpha, fadeDuration);
    }

    public void FadeInOut(float fadeDuration)
    {       
        FadeIn(fadeDuration);
        FadeOut(fadeDuration);        
    }
}
