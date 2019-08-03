using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile
{
    public Vector2Int Pos { get; set; }
    public bool IsAccessible { get; set; }
    public bool IsBlocking { get; set; }
    public bool IsSliding { get; set; }

    public GameObject entityOnTop;

    public GameTile(Vector2Int position, 
        bool isAccessible = false,
        bool isBlocking = false,
        bool IsSliding = false)
    {
        Pos = position;
        this.IsAccessible = isAccessible;
        this.IsBlocking = isBlocking;
        this.IsSliding = IsSliding;
    }

    public GameTile GetTileRelativeToMe(int xOffset, int yOffset)
    {
        Vector2Int offsetTilePos = new Vector2Int(Pos.x + xOffset, Pos.y + yOffset);
        
        return GameGrid.Instance.GetTileAtposition(offsetTilePos);
    }
}
