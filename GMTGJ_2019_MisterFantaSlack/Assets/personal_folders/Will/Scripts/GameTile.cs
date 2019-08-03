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

    public GameTile GetTileRelativeToMe(EDirection direction)
    {

        int xOffset = 0, yOffset = 0;

        switch(direction)
        {
            case EDirection.Up:
                xOffset = 0;
                yOffset = 1;
                break;
            case EDirection.Down:
                xOffset = 0;
                yOffset = -1;
                break;
            case EDirection.Left:
                xOffset = -1;
                yOffset = 0;
                break;
            case EDirection.Right:
                xOffset = 1;
                yOffset = 0;
                break;
        }
        Vector2Int offsetTilePos = new Vector2Int(Pos.x + xOffset, Pos.y + yOffset);

        GameTile tile = GameGrid.Instance.GetTileAtposition(offsetTilePos);

        if (tile != null)
        {
            return tile;
        }

        Debug.LogWarning(direction +" relative of tile (" + Pos.x + ", " + Pos.y + ") isn't a tile");

        return null;
    }
}
