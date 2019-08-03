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

    void Start() {
        Tilemap tilemap = GetComponent<Tilemap>();

        tilemap.CompressBounds();

        GameGrid.Instance.BuildGrid(tilemap);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Input.mousePosition;
            Vector3 worldPointPos = Camera.main.ScreenToWorldPoint(position);
            Debug.Log(GameGrid.TranslateToGridCoordinates(worldPointPos));
            //grid.GetTileAtposition(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector3Int());
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
