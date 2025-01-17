﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile
{
    public readonly Vector2Int Pos;
    public bool IsAccessible { get; set; }
    public bool IsBlocking { get; set; }
    public bool IsSliding { get; set; }
    public bool IsRotating { get; set; }

    GameObject _entityOnTop;
    public GameObject entityOnTop
    {
        get { return _entityOnTop; }
        set
        {
            if(Pos.x == 2 && Pos.y == 5)
            {
                //Debug.Log($"setter entity on top: {value}");
            }
            _entityOnTop = value;
        }
    }

    public GameTile(Vector2Int position,
        bool isAccessible = false,
        bool isBlocking = false,
        bool IsSliding = false,
        bool IsRotating = false)
    {
        Pos = position;
        this.IsAccessible = isAccessible;
        this.IsBlocking = isBlocking;
        this.IsSliding = IsSliding;
        this.IsRotating = IsRotating;
    }

    public GameTile GetTileRelativeToMe(EDirection direction)
    {

        int xOffset = 0, yOffset = 0;

        switch (direction)
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

        Debug.LogWarning(direction + " relative of tile (" + Pos.x + ", " + Pos.y + ") isn't a tile");

        return null;
    }
}
