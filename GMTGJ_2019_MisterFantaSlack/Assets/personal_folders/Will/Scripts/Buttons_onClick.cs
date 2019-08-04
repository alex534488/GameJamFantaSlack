using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons_onClick : MonoBehaviour
{
   
    public void Restart()
    {
        Debug.Log("Restart!");
        InputManager.Instance.StartReset();
    }

    public void GoToMainMenu()
    {
        Debug.Log("GoToMainMenu!");
        InputManager.Instance.StartPause();
    }

    public void Skip()
    {
        Debug.Log("Skip to next level.");
        GameManager.Instance.LevelCompleted();
    }

    public void OrderFire()
    {
        InputManager.Instance.StartFire();
    }

    public void OrderUp()
    {
        Debug.Log("OrderUp!");
        InputManager.Instance.StartMove(EDirection.Up);
    }

    public void OrderDown()
    {
        Debug.Log("OrderDown!");
        InputManager.Instance.StartMove(EDirection.Down);
    }

    public void OrderLeft()
    {
        Debug.Log("OrderLeft!");
        InputManager.Instance.StartMove(EDirection.Left);
    }

    public void OrderRight()
    {
        Debug.Log("OrderRight!");
        InputManager.Instance.StartMove(EDirection.Right);
    }
}
