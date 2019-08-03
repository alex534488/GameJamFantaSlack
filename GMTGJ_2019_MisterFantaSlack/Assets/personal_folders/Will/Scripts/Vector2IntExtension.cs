using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2IntExtension 
{
    public static Vector3Int ToVector3Int(this Vector2Int vector)
    {
        return new Vector3Int(vector.x, vector.y, 0);
    }
}
