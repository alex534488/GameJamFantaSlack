using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
	public GameObject DestructSoundPrefab;

	public virtual void DestructObject()
    {
		GameObject newObject = Instantiate(DestructSoundPrefab, transform.position, transform.rotation);
		GameGrid.Instance.RemoveMyselfFromTile(gameObject);
        gameObject.SetActive(false);
    }
}
