using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Audio : MonoBehaviour
{
	public AudioPlayable Playable;
	public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
		Playable.PlayOn(source);
    }
}
