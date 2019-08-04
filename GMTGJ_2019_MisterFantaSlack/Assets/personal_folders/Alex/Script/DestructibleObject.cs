using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public virtual void DestructObject()
    {
        GameGrid.Instance.RemoveMyselfFromTile(gameObject);
        gameObject.SetActive(false);
    }
}
