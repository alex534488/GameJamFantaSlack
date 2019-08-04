using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public void DestructObject()
    {
        GameGrid.Instance.RemoveMyselfFromTile(gameObject);
        Destroy(gameObject);
    }
}
