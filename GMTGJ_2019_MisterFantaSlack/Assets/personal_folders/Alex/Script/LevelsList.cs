using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsList", menuName = "ScriptableObjects/LevelsList", order = 2)]
public class LevelsList : ScriptableObject
{
    public List<string> levelSceneName = new List<string>();
}
