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
        BoundsInt cellBounds = tilemap.cellBounds;

        Instance.width = cellBounds.xMax - cellBounds.xMin;
        Instance.height = cellBounds.yMax - cellBounds.yMin;

        Instance.GameTiles = new List<GameTile>();

        for (int x = cellBounds.xMin; x < cellBounds.xMax; ++x)
        {
            for (int y = cellBounds.yMin; y < cellBounds.yMax; ++y)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(pos))
                {
                    pos.x += width/2;
                    pos.y += height/2;
                    Debug.Log(pos);
                    Instance.GameTiles.Add(new GameTile(pos));
                }
            }
        }
        Debug.Log(Instance.GameTiles.Count);
    }

    public GameTile GetTileAtposition(Vector3Int position)
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

    public static Vector3Int TranslateToGridCoordinates(Vector3 coord)
    {
        coord.x += Instance.width / 2;
        coord.y += Instance.height / 2;

        return Vector3Int.FloorToInt(coord);
    }
}
