using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimAnimation : MonoBehaviour
{
    public static KimAnimation Instance = null;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void AnimationAngryKim()
    {
        GetComponent<Animator>().Play("Angry");
    }
}
