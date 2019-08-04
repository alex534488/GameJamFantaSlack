using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : DestructibleObject
{
    public override void DestructObject()
    {
        base.DestructObject();

        GameGrid.Instance.GetTileAtposition(transform.position).IsAccessible = true;
    }
}
