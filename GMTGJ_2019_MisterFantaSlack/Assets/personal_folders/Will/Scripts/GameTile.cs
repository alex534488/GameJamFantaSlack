﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile
{
    public Vector2Int Pos { get; set; }
    public bool IsAccessible { get; set; }
    public bool IsDestructible { get; set; }
    public bool IsBlocking { get; set; }

    public GameTile(Vector2Int position, 
        bool isAccessible = false,
        bool isDestructible = false,
        bool isBlocking = false)
    {
        Pos = position;
        IsAccessible = isAccessible;
        IsDestructible = isDestructible;
        IsBlocking = isBlocking;
    }

    public GameTile GetTileRelativeToMe(int xOffset, int yOffset)
    {
        Vector2Int offsetTilePos = new Vector2Int(Pos.x + xOffset, Pos.y + yOffset);
        
        return GameGrid.Instance.GetTileAtposition(offsetTilePos);
    }
}
