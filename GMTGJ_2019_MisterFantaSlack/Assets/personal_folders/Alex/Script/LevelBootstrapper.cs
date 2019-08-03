using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBootstrapper : MonoBehaviour
{
    [Tooltip("")]
    public int levelNumber = 0;
    public SceneLinks sceneLinks;

    private static bool hasStarted = false;

    void Awake()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            PlayerPrefs.SetInt(SaveKeys.MAX_LEVEL_REACHED, levelNumber);
            SceneManager.LoadScene(sceneLinks.PersistantGameScene, LoadSceneMode.Single);
        }
    }
}
