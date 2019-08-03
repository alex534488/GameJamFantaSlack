using System.Collections;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameGrid : MonoBehaviour
{
    public static GameGrid Instance = null;

    private int width, height;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public List<GameTile> GameTiles;

    public void BuildGrid(Tilemap tilemap)
    {
        tilemap.CompressBounds();
        BoundsInt cellBounds = tilemap.cellBounds;

        Instance.width = cellBounds.xMax - cellBounds.xMin;
        Instance.height = cellBounds.yMax - cellBounds.yMin;

        Instance.GameTiles = new List<GameTile>();
        Debug.Log("Origin = (" + cellBounds.xMin + ", " + cellBounds.yMin + ")");
        Debug.Log("Middle = (" + (cellBounds.xMin + Instance.width / 2) + ", " + (cellBounds.yMin + Instance.height / 2)  + ")");
        Debug.Log("Width: " + Instance.width + ", Height: " + Instance.height);

        for (int x = cellBounds.xMin; x < cellBounds.xMax; ++x)
        {
            for (int y = cellBounds.yMin; y < cellBounds.yMax; ++y)
            {
                Vector2Int pos = new Vector2Int(x, y);
                
                if (tilemap.HasTile(pos.ToVector3Int()))
                {
                    pos.x += width/2;
                    pos.y += height/2;
                    GameTile IdentifiedTile = Instance.IdentifyGameTile((tilemap.GetTile(pos.ToVector3Int()) as Tile));
                    Instance.GameTiles.Add(IdentifiedTile);
                }
            }
        }
    }

    public GameTile IdentifyGameTile(Tile tile)
    {
        //TODO: Get info from tile and add it to a GameTile.
        return new GameTile(Vector2Int.zero);
    }

    public GameTile GetTileAtposition(Vector2Int position)
    {
        foreach(GameTile tile in GameTiles)
        {
            if (tile.Pos == position)
            {
                return tile;
            }
        }
        return null;
    }

    public GameTile GetTileAtposition(Vector2 position)
    {
        Vector2Int realPos = ToGridCoordinates(position);
        foreach (GameTile tile in GameTiles)
        {
            //Debug.Log("tile.Pos: " + tile.Pos + ", realPos: " + realPos);
            if (tile.Pos == realPos)
            {
                return tile;
            }
        }
        return null;
    }

    //use this if you want to have a grid coordinate from, let's say, a tilemap coordinate
    public static Vector2Int ToGridCoordinates(Vector2 coord)
    {
        coord.x += Instance.width / 2;
        coord.y += Instance.height / 2;

        return Vector2Int.FloorToInt(coord);
    }

    //use this if you want to have a tilemap coordinate with a grid one.
    public static Vector3Int ToWorldCoordinates(Vector2Int coord)
    {
        Vector2 worldCoord = new Vector2(coord.x - Instance.width / 2, coord.y - Instance.height / 2);
        return Vector3Int.FloorToInt(worldCoord);
    }
}
