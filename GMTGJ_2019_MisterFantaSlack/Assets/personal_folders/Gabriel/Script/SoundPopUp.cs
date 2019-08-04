using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

public class SoundPopUp : MonoBehaviour
{
	public AudioClip Clip;
	public AudioSource Source;

	private void Awake()
	{
		float Delay = Random.Range(0.0f, 0.1f);
		Source.clip = Clip;
		Source.PlayDelayed(Delay);

		Delay += Source.clip.length;

		Invoke("SoundEnd", Delay);
	}

	// Start is called before the first frame update
	void Start()
    {
		
    }

	void SoundEnd()
	{
		Destroy(gameObject);
	}
}
