using System.Collections;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameGrid : MonoBehaviour
{
    public const float GRID_CELL_OFFSET = 0.5f;
    public static GameGrid Instance = null;

    public TileIdentifier tileIdentifier;

    public int width, height;

    public Vector2 minBounds;
    public Vector2 maxBounds;

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

        minBounds = new Vector2(cellBounds.xMin, cellBounds.yMin);
        maxBounds = new Vector2(cellBounds.xMax, cellBounds.yMax);

        Instance.GameTiles = new List<GameTile>();
        Debug.Log("min = " + minBounds);
        Debug.Log("max = " + maxBounds);
        Debug.Log("Middle = (" + (cellBounds.xMin + Instance.width / 2) + ", " + (cellBounds.yMin + Instance.height / 2)  + ")");

        Bounds bounds = new Bounds(new Vector2(cellBounds.xMin, cellBounds.yMin), new Vector2(Instance.width, Instance.height));

        for (int x = cellBounds.xMin; x < cellBounds.xMax; ++x)
        {
            for (int y = cellBounds.yMin; y < cellBounds.yMax; ++y)
            {
                Vector2Int pos = new Vector2Int(x, y);
                
                if (tilemap.HasTile(pos.ToVector3Int()))
                {
                    Tile tile = (tilemap.GetTile(pos.ToVector3Int()) as Tile);
                    Vector2Int gameTilePos = new Vector2Int(pos.x + width / 2, pos.y + height / 2);
                    GameTile IdentifiedTile = Instance.IdentifyGameTile(tile, gameTilePos, pos.ToVector3Int(), tilemap);
                    Instance.GameTiles.Add(IdentifiedTile);
                }
            }
        }
    }

    public GameTile IdentifyGameTile(Tile tile, Vector2Int gameTilePosition, Vector3Int tilePosition, Tilemap tilemap)
    {
        if (tile == null)
            return null;

        TileIdentifier.TileData data = tileIdentifier.GetData(tile.sprite);

        GameTile gameTile = new GameTile(gameTilePosition, data.accessible, data.blocking, data.slide, data.rotates);

        //HACK : GRID_CELL_OFFSET est la moitié de la taille d'une tile, c'est pour placer l'objet en son millieu
        Vector2 worldPosition = new Vector3((gameTilePosition.x - Instance.width / 2) + GRID_CELL_OFFSET, (gameTilePosition.y - Instance.height / 2) + GRID_CELL_OFFSET, 1);
      
        gameTile.entityOnTop = EntitySpawner.Instance.SpawnEntity(data.entityOnTop, worldPosition, tilemap.GetTransformMatrix(tilePosition).rotation);

        if (data.newTileSprite != null)
        {
            Tile newTile = ScriptableObject.CreateInstance<Tile>();

            newTile.sprite = data.newTileSprite;

            tilemap.SetTile(tilePosition, newTile);
        }

        return gameTile;
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
            if (tile.Pos == realPos)
            {
                return tile;
            }
        }
        return null;
    }

    public void RemoveMyselfFromTile(GameObject myObj)
    {
        GameTile currentTile = GetTileAtposition(myObj.transform.position);
        currentTile.entityOnTop = null;
    }

    public void AddMyselfOnTile(GameObject myObj)
    {
        GameTile currentTile = GetTileAtposition(myObj.transform.position);
        currentTile.entityOnTop = myObj;
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

    //To place object centered on a tile
    public static Vector3 GetCenterCellPosition(GameTile tile)
    {
        return new Vector2((tile.Pos.x - Instance.width / 2) + GRID_CELL_OFFSET, (tile.Pos.y - Instance.height / 2) + GRID_CELL_OFFSET);
    }
}
