using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridScript : MonoBehaviour
{
    Tilemap tileMap;

    private List<Vector3> availablePlaces;
    [SerializeField]
    private int nbTileinGrid;

    void Start() {
        Tilemap tileMap = GetComponent<Tilemap>();
        availablePlaces = new List<Vector3>();

        foreach(Vector3Int pos in tileMap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = pos;

            if (tileMap.HasTile(pos))
            {
                nbTileinGrid++;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
