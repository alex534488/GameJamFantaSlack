using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneLinks", menuName = "ScriptableObjects/SceneLinks", order = 1)]
public class SceneLinks : ScriptableObject
{
    public string MainMenu = "";
    public string PersistantGameScene = "";
}
