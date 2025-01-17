﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

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
    public EntitySpawner entitySpawner;

    // EXTERNAL FIELD

    public Camera mainCamera;

    // FLOW

    public SceneLinks sceneLinks;
    public LevelsList levelList;

    public UnityEvent beforeGameStart = new UnityEvent();
    public UnityEvent gameStarted = new UnityEvent();
    public UnityEvent levelOver = new UnityEvent();

    public bool levelCompleted = false;

    // LOCAL DATA

    private int currentLevel;

    private bool canRestart = false;

    private bool returningHome = false;

    private bool wasBombRestart = false;

    private bool wasRestart = false;

    void Start()
    {
        canRestart = false;
        returningHome = false;
        levelCompleted = false;
        wasBombRestart = false;
        wasRestart = false;

        currentLevel = PlayerPrefs.GetInt(SaveKeys.MAX_LEVEL_REACHED, -1);

        if(currentLevel <= -1)
        {
            currentLevel = 0;
            PlayerPrefs.SetInt(SaveKeys.MAX_LEVEL_REACHED, currentLevel);
        }

        LoadNextLevel();
    }

    public void Restart()
    {
        if (canRestart)
        {
            canRestart = false;

            wasRestart = true;

            levelCompleted = true;

            ui.FadeOut(0.5f, delegate ()
            {
                SceneManager.UnloadSceneAsync(levelList.levelSceneName[currentLevel]).completed += RestartCompleted;

                if (levelOver != null)
                    levelOver.Invoke();
            });
        }
    }

    public void RestartBombe()
    {
        if (canRestart)
        {
            wasBombRestart = true;

            canRestart = false;

            levelCompleted = true;

            ui.FadeOutWhite(0.25f, delegate ()
            {
                SceneManager.UnloadSceneAsync(levelList.levelSceneName[currentLevel]).completed += RestartCompleted;

                if (levelOver != null)
                    levelOver.Invoke();
            });
        }
    }

    public void ReturnToHome()
    {
        if (returningHome)
            return;

        returningHome = true;

        // Return to Home Screen
        SceneManager.LoadScene(sceneLinks.MainMenu, LoadSceneMode.Single);
    }

    public void LevelCompleted()
    {
        canRestart = false;

        levelCompleted = true;
        
        GameManager.Instance.ui.kimBubble.Say(KimMessageType.LevelCompleted, false, 2f);

        this.DelayedCall(1, delegate ()
        {
            ui.FadeOut(0.5f, delegate ()
            {
                SceneManager.UnloadSceneAsync(levelList.levelSceneName[currentLevel]).completed += OnNextLevelReady;

                if (levelOver != null)
                    levelOver.Invoke();
            });
        });

        // SFX
        winSFX?.PlayOn(GetComponent<AudioSource>());
    }
    public AudioAsset winSFX;


    public void SkipLevel()
    {
        canRestart = false;

        levelCompleted = true;

        this.DelayedCall(1, delegate ()
        {
            ui.FadeOut(0.5f, delegate ()
            {
                SceneManager.UnloadSceneAsync(levelList.levelSceneName[currentLevel]).completed += OnNextLevelReady;

                if (levelOver != null)
                    levelOver.Invoke();
            });
        });
    }

    private void LoadNextLevel()
    {
        if (currentLevel >= levelList.levelSceneName.Count)
        {
            // GAME OVER (GAME END)
            PlayerPrefs.SetInt(SaveKeys.MAX_LEVEL_REACHED, 0); // Reset Saves

            ui.ShowEndCredits(delegate () {
                SceneManager.LoadSceneAsync(sceneLinks.MainMenu, LoadSceneMode.Single);
            });
        }
        else
        {
            // NEXT LEVEL
            SceneManager.LoadSceneAsync(levelList.levelSceneName[currentLevel], LoadSceneMode.Additive).completed += LevelLoaded;
        }
    }

    private void SetupGrid()
    {
        // Search for the grid !
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene currentScene = SceneManager.GetSceneAt(i);

            GameObject[] rootGameObjects = currentScene.GetRootGameObjects();
            for (int j = 0; j < rootGameObjects.Length; j++)
            {
                Grid grid = rootGameObjects[j].GetComponent<Grid>();
                if (grid != null && rootGameObjects[j].name == "Grid")
                {
                    Tilemap currentTileMap = grid.GetComponentInChildren<Tilemap>();
                    if (currentTileMap != null)
                    {
                        // Found it ! Build the Game Grid (backend)
                        GameGrid.Instance.BuildGrid(currentTileMap);
                        return;
                    }
                }
            }
        }

        Debug.Log("Error, no grid in level");
    }

    private void SetupCamera()
    {
        // Search for the grid !
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene currentScene = SceneManager.GetSceneAt(i);

            GameObject[] rootGameObjects = currentScene.GetRootGameObjects();
            for (int j = 0; j < rootGameObjects.Length; j++)
            {
                if (rootGameObjects[j].tag == "ScopeCamera")
                {
                    Camera scopeCamera = rootGameObjects[j].GetComponent<Camera>();
                    if(scopeCamera != null)
                    {
                        mainCamera.orthographicSize = scopeCamera.orthographicSize;
                        mainCamera.transform.position = scopeCamera.transform.position;
                        scopeCamera.gameObject.SetActive(false);
                    }
                }
            }
        }

        Debug.Log("Error, no camera in level");
    }

    // SCENE MANAGEMENT ASYNC COMPLETE CALLBACKS

    private void LevelLoaded(AsyncOperation obj)
    {
        if (beforeGameStart != null)
            beforeGameStart.Invoke();

        levelCompleted = false;

        SetupGrid();

        SetupCamera();

        if (wasBombRestart)
        {
            ui.FadeInWhite(3.5f, delegate ()
            {
                if (gameStarted != null)
                    gameStarted.Invoke();

                canRestart = true;

                KimSeesLevelAgain();
            });
        }
        else
        {
            ui.FadeIn(0.5f, delegate ()
            {
                if (gameStarted != null)
                    gameStarted.Invoke();

                if (wasRestart)
                {
                    KimSeesLevelAgain();
                }

                canRestart = true;
            });
        }

        wasBombRestart = false;
        wasRestart = false;
    }

    private void OnNextLevelReady(AsyncOperation obj)
    {
        currentLevel++;
        PlayerPrefs.SetInt(SaveKeys.MAX_LEVEL_REACHED, currentLevel);

        KimSeesNewLevel();

        LoadNextLevel();
    }

    private void RestartCompleted(AsyncOperation obj)
    {
        SceneManager.LoadSceneAsync(levelList.levelSceneName[currentLevel], LoadSceneMode.Additive).completed += LevelLoaded;
    }

    private void KimSeesNewLevel()
    {
        this.DelayedCall(0.5f,()=>{ GameManager.Instance.ui.kimBubble.Say(KimMessageType.NewLevel, false, 2f); });
    }

    private void KimSeesLevelAgain()
    {
        this.DelayedCall(0.5f,()=>{ GameManager.Instance.ui.kimBubble.Say(KimMessageType.Restart, false, 2f); });
    }
}
