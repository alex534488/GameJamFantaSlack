﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup canvas;
    public float fadeInAnimationDuration = 2;

    public TextMeshProUGUI startGameText;

    public SceneLinks sceneLinks;

    private bool interactable = false;

    void Start()
    {
        interactable = false;
        canvas.DOFade(1, fadeInAnimationDuration).OnComplete(delegate () {
            interactable = true;
        });
    }

    public void Continue()
    {
        if (!interactable)
            return;

        LevelBootstrapper.startingFromMainMenu = true;

        interactable = false;
        canvas.DOFade(0, fadeInAnimationDuration).OnComplete(delegate () {
            SceneManager.LoadScene(sceneLinks.PersistantGameScene, LoadSceneMode.Single);
        });
    }

    public void ResetSaves()
    {
        if (!interactable)
            return;

        PlayerPrefs.SetInt(SaveKeys.MAX_LEVEL_REACHED, 0);
    }

    public void SetLevelToStart(int levelNumber)
    {
        PlayerPrefs.SetInt(SaveKeys.MAX_LEVEL_REACHED, levelNumber);
        startGameText.text = "Start";
    }

    public void Quit()
    {
        if (!interactable)
            return;

        interactable = false;

        Debug.Log("Leaving the game");

#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
        Application.Quit();
#elif (UNITY_WEBGL)
        Application.OpenURL("about:blank");
#endif
    }
}
