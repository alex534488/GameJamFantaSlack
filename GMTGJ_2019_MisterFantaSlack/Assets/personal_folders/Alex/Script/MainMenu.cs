using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup canvas;
    public float fadeInAnimationDuration = 2;

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

        interactable = false;
        canvas.DOFade(0, fadeInAnimationDuration).OnComplete(delegate () {
            SceneManager.LoadScene(sceneLinks.PersistantGameScene, LoadSceneMode.Single);
        });
    }

    public void OpenLevelSelection()
    {
        if (!interactable)
            return;

        // Open level selection screen
    }

    public void SelectLevel(int number)
    {

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
