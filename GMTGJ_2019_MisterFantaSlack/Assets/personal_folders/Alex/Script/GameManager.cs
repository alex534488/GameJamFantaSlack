using System.Collections;
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

    // FLOW

    public SceneLinks sceneLinks;
    public LevelsList levelList;

    public UnityEvent beforeGameStart = new UnityEvent();
    public UnityEvent gameStarted = new UnityEvent();
    public UnityEvent levelOver = new UnityEvent();

    private int currentLevel;

    private bool canRestart = false;

    private bool returningHome = false;

    void Start()
    {
        canRestart = false;
        returningHome = false;

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

            if (levelOver != null)
                levelOver.Invoke();

            ui.FadeOut(0.5f, delegate ()
            {
                SceneManager.UnloadSceneAsync(levelList.levelSceneName[currentLevel]).completed += RestartCompleted;
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

        if(levelOver != null)
            levelOver.Invoke();

        ui.FadeOut(0.5f, delegate ()
        {
            SceneManager.UnloadSceneAsync(levelList.levelSceneName[currentLevel]).completed += OnNextLevelReady;
        });
    }

    private void LoadNextLevel()
    {
        if (currentLevel >= levelList.levelSceneName.Count)
        {
            // GAME OVER (GAME END)
            PlayerPrefs.SetInt(SaveKeys.MAX_LEVEL_REACHED, 0); // Reset Saves
            SceneManager.LoadScene(sceneLinks.MainMenu, LoadSceneMode.Single);
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
                if (grid != null)
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

    // SCENE MANAGEMENT ASYNC COMPLETE CALLBACKS

    private void LevelLoaded(AsyncOperation obj)
    {
        if (beforeGameStart != null)
            beforeGameStart.Invoke();

        SetupGrid();

        ui.FadeIn(0.5f, delegate ()
        {
            if (gameStarted != null)
                gameStarted.Invoke();

            canRestart = true;
        });
    }

    private void OnNextLevelReady(AsyncOperation obj)
    {
        currentLevel++;
        PlayerPrefs.SetInt(SaveKeys.MAX_LEVEL_REACHED, currentLevel);

        LoadNextLevel();
    }

    private void RestartCompleted(AsyncOperation obj)
    {
        SceneManager.LoadSceneAsync(levelList.levelSceneName[currentLevel], LoadSceneMode.Additive).completed += LevelLoaded;
    }
}
