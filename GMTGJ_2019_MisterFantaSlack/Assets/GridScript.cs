using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CCC;

public class GridScript : MonoBehaviour
{
    private List<Tile> tiles;

    [SerializeField]
    private int nbTileInGrid;

    private Tilemap tilemap;

    void Start() {
        tilemap = GetComponent<Tilemap>();

        tilemap.CompressBounds();

        GameGrid.Instance.BuildGrid(tilemap);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Input.mousePosition;
            Vector2 worldPointPos = Camera.main.ScreenToWorldPoint(position);
            GameTile tile = GameGrid.Instance.GetTileAtposition(worldPointPos);
            if (tile != null)
            {
                Vector2Int gridPos = GameGrid.Instance.GetTileAtposition(worldPointPos).Pos;
                Vector3Int tilePosToWorld = GameGrid.ToWorldCoordinates(gridPos);

                Debug.Log("worldPos: " + worldPointPos 
                    + ", gridPos: " + gridPos 
                    + ", tileToWorldpos: " + tilePosToWorld
                    + ", hasTile: " + tilemap.HasTile(tilePosToWorld)
                    + ", GridToWorldToGrid: " + GameGrid.ToGridCoordinates(new Vector2(tilePosToWorld.x, tilePosToWorld.y)));
            }
            else
            {
                Debug.Log("No tile there.");
            }
        }
    }

    
}

public static class Vector3Extension
{
    public static Vector3Int ToVector3Int(this Vector3 vector)
    {
        int x = vector.x.RoundedToInt();
        return Vector3Int.zero;
    }
}
