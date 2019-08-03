using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public enum Entity
    {
        destructible,
        soldier,
        cible
    }

    public GameObject destructiblePrefab;
    public GameObject soldierPrefab;
    public GameObject ciblePrefab;

    public void SpawnEntity(Entity entity, Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn;

        switch (entity)
        {
            case Entity.destructible:
                objectToSpawn = destructiblePrefab;
                break;
            case Entity.soldier:
                objectToSpawn = soldierPrefab;
                break;
            case Entity.cible:
                objectToSpawn = ciblePrefab;
                break;
            default:
                objectToSpawn = null;
                break;
        }

        if(objectToSpawn != null)
        {
            Instantiate(objectToSpawn, position, rotation);
        }
    }
}
