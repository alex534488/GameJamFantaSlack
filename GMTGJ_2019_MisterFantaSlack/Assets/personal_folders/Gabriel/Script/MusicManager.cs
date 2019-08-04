using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	public AudioPlayable MusicPlayable;
	public AudioSource source;

	// Start is called before the first frame update
	void Start()
    {
		if (source == null)
			source = GetComponent<AudioSource>();

		MusicPlayable.PlayLoopedOn(source);
    }
}
