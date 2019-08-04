using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlexTestScript : MonoBehaviour
{
    public AudioPlayable sound;
    public AudioAssetGroup groupSound;
    public AudioSource source;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            sound.PlayOn(source);
            groupSound.PlayOn(source);
        }
    }
}
