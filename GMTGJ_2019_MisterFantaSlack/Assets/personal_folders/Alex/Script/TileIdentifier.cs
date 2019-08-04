using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileIdentifier", menuName = "ScriptableObjects/TileIdentifier", order = 3)]
public class TileIdentifier : ScriptableObject
{
    [System.Serializable]
    public class TileData
    {
        // Identifier
        public Sprite sprite;

        // Data
        public bool accessible;
        public bool blocking;
        public bool slide;
        public bool rotates;
        public Sprite newTileSprite;
        public EntitySpawner.Entity entityOnTop;
    }

    public List<TileData> tilesData = new List<TileData>();

    public TileData GetData(Sprite currentSprite)
    {
        foreach (TileData tileData in tilesData)
        {
            if (tileData.sprite == currentSprite)
            {
                return tileData;
            }
        }

        return null;
    }
}
