using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // SINGLETON

    public static GameManager Instance = null;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // MANAGERS

    public UIManager ui;

    // FLOW

    public SceneLinks sceneLinks;
    public LevelsList levelList;

    public UnityEvent beforeGameStart = new UnityEvent();
    public UnityEvent gameStarted = new UnityEvent();

    private int currentLevel;

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt(SaveKeys.MAX_LEVEL_REACHED, -1);

        if(currentLevel <= -1)
        {
            currentLevel = 0;
            PlayerPrefs.SetInt(SaveKeys.MAX_LEVEL_REACHED, currentLevel);
        }

        // GAME OVER (GAME END)
        if(currentLevel >= levelList.levelSceneName.Count)
        {
            SceneManager.LoadScene(sceneLinks.MainMenu, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadSceneAsync(levelList.levelSceneName[currentLevel], LoadSceneMode.Additive).completed += LevelLoaded;
        }
    }

    public void Restart()
    {
        ui.FadeOut(0.5f, delegate ()
        {
            SceneManager.UnloadSceneAsync(levelList.levelSceneName[currentLevel]).completed += RestartCompleted;
        });
    }


    // SCENE MANAGEMENT ASYNC COMPLETE CALLBACKS

    private void LevelLoaded(AsyncOperation obj)
    {
        if (beforeGameStart != null)
            beforeGameStart.Invoke();

        ui.FadeIn(0.5f, delegate ()
        {
            if (gameStarted != null)
                gameStarted.Invoke();
        });
    }

    private void RestartCompleted(AsyncOperation obj)
    {
        SceneManager.LoadSceneAsync(levelList.levelSceneName[currentLevel], LoadSceneMode.Additive).completed += LevelLoaded;
    }
}
