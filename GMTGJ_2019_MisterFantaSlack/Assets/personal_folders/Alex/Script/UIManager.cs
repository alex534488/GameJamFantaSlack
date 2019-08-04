using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public Image blackFullscreen;

    public KimBubble kimBubble;

    public CanvasGroup finalThanks;

    public void FadeIn(float duration = 1, UnityAction onComplete = null)
    {
        Fade(0,duration,onComplete);
    }

    public void FadeOut(float duration = 1, UnityAction onComplete = null)
    {
        Fade(1, duration, onComplete);
    }

    public void Fade(float end, float duration = 1, UnityAction onComplete = null)
    {
        blackFullscreen.DOFade(end, duration).OnComplete(() => {
            if (onComplete != null)
                onComplete();
        });
    }

    public void ShowEndCredits(UnityAction onComplete = null)
    {
        if(finalThanks != null)
        {
            finalThanks.DOFade(1, 1).OnComplete(delegate () {
                this.DelayedCall(5, delegate () {
                    finalThanks.DOFade(0, 1).OnComplete(delegate () {
                        if (onComplete != null)
                            onComplete();
                    });
                });
            });
        }
        else
        {
            if (onComplete != null)
                onComplete();
        }
    }
}
