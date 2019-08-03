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

    public void ShootingCompleted()
    {
        if(targetLeft > 0)
        {
            foreach (GameObject gameObjects in EntitySpawner.Instance.cibleObjects)
            {
                Cible cible = gameObjects.GetComponent<Cible>();
                if(cible != null)
                {
                    cible.Respawn();
                }
            }
        }
        else
        {
            GameManager.Instance.LevelCompleted();
        }
    }
}
