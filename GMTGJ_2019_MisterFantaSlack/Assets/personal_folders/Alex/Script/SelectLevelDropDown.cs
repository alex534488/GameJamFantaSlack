using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectLevelDropDown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public LevelsList levelList;

    void Start()
    {
        List<string> options = new List<string>();

        options.Add("Choose Level");

        foreach (string level in levelList.levelSceneName)
        {
            options.Add(level);
        }

        dropdown.ClearOptions();

        dropdown.AddOptions(options);
    }
}
