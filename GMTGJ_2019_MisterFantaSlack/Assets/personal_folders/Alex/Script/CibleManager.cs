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
        if(targetLeft > 0)
        {
            targetLeft = amountOfTarget;

            GameManager.Instance.ui.kimBubble.Say("Failure!",true, focusAnimDuration);

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
            GameManager.Instance.LevelCompleted();
        }
    }
}
