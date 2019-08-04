using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CibleManager : MonoBehaviour
{
    // SINGLETON

    public static CibleManager Instance = null;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // DATA

    public float focusAnimDuration = 2;

    private int amountOfTarget;
    private int targetLeft;

    public void SetTargetAmountInLevel(int amount)
    {
        amountOfTarget = amount;
        targetLeft = amountOfTarget;
    }

    public void TargetShotted()
    {
        targetLeft--;
    }

    public void ShootingCompleted(Action onComplete)
    {
        if (GameManager.Instance.levelCompleted)
        {
            if (onComplete != null)
                onComplete();

            return;
        }

        if(targetLeft > 0)
        {
            // but we hit at least one
            if(targetLeft < amountOfTarget)
            {
                targetLeft = amountOfTarget;

                GameManager.Instance.ui.kimBubble.Say(KimMessageType.Failure, true, focusAnimDuration);

                foreach (GameObject gameObjects in EntitySpawner.Instance.cibleObjects)
                {
                    Cible cible = gameObjects.GetComponent<Cible>();
                    if (cible != null && cible.gameObject.activeSelf)
                    {
                        cible.FocusAnim(focusAnimDuration);
                    }
                }

                this.DelayedCall(focusAnimDuration, delegate () {
                    foreach (GameObject gameObjects in EntitySpawner.Instance.cibleObjects)
                    {
                        Cible cible = gameObjects.GetComponent<Cible>();
                        if (cible != null)
                        {
                            cible.Respawn();
                        }
                    }

                    if (onComplete != null)
                        onComplete();
                });
            }
            else
            {
                if (onComplete != null)
                    onComplete();
            }
        }
        else
        {
            GameManager.Instance.LevelCompleted();
        }
    }
}
