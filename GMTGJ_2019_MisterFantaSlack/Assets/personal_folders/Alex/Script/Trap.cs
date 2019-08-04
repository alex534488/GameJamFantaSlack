using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
	public GameObject TrapSoundPrefab;

	public void DestructObject()
    {
		GameObject newObject = Instantiate(TrapSoundPrefab, transform.position, transform.rotation);
		GameGrid.Instance.RemoveMyselfFromTile(gameObject);
        Destroy(gameObject);
    }
}
