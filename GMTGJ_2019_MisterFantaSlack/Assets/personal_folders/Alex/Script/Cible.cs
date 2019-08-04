using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cible : DestructibleObject
{
    public float scaleUpAnimDuration = 0.2f;

    public Tween focusAnim;

    public override void DestructObject()
    {
        base.DestructObject();

        if(CibleManager.Instance != null)
        {
            CibleManager.Instance.TargetShotted();
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        GameGrid.Instance.AddMyselfOnTile(gameObject);
    }

    public void FocusAnim(float duration, Action onComplete = null)
    {
        this.DelayedCall(duration, delegate() {
            StopFocus();
            if (onComplete != null)
                onComplete();
        });

        focusAnim = transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), scaleUpAnimDuration).SetLoops(-1,LoopType.Yoyo);
    }

    public void StopFocus()
    {
        focusAnim.Kill();
        transform.DOScale(new Vector3(1, 1, 1), scaleUpAnimDuration);
    }
}
