using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBootstrapper : MonoBehaviour
{
    public SceneLinks sceneLinks;
    public LevelsList levelList;

    private static bool hasStarted = false;

    void Awake()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            PlayerPrefs.SetInt(SaveKeys.MAX_LEVEL_REACHED, levelList.GetIndexOfLevel(SceneManager.GetActiveScene().name));
            SceneManager.LoadScene(sceneLinks.PersistantGameScene, LoadSceneMode.Single);
        }
    }
}
