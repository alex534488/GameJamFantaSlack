using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{

    // SINGLETON

    public static EntitySpawner Instance = null;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public float delayToWaitAfterShot = 1;

    void Start()
    {
        GameManager.Instance.gameStarted.AddListener(OnGameStart);
        GameManager.Instance.levelOver.AddListener(OnLevelEnd);
    }

    public void OnGameStart()
    {
        CibleManager.Instance.SetTargetAmountInLevel(cibleObjects.Count);
    }

    public void OnLevelEnd()
    {
        for (int i = 0; i < destructibleObjects.Count; i++)
        {
            Destroy(destructibleObjects[i]);
        }

        destructibleObjects.Clear();

        for (int i = 0; i < soldierObjects.Count; i++)
        {
            Destroy(soldierObjects[i]);
        }

        soldierObjects.Clear();

        for (int i = 0; i < cibleObjects.Count; i++)
        {
            Destroy(cibleObjects[i]);
        }

        cibleObjects.Clear();

        for (int i = 0; i < trapObjects.Count; i++)
        {
            Destroy(trapObjects[i]);
        }

        trapObjects.Clear();
    }

    public enum Entity
    {
        none,
        destructible,
        soldier,
        cible,
        trap
    }

    [HideInInspector]
    public List<GameObject> destructibleObjects = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> soldierObjects = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> cibleObjects = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> trapObjects = new List<GameObject>();

    public GameObject destructiblePrefab;
    public GameObject soldierPrefab;
    public GameObject ciblePrefab;
    public GameObject trapPrefab;

    public int bulletsAlive = 0;

    public GameObject SpawnEntity(Entity entity, Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn;
        List<GameObject> listToAddTo = null;

        switch (entity)
        {
            case Entity.destructible:
                objectToSpawn = destructiblePrefab;
                listToAddTo = destructibleObjects;
                break;

            case Entity.soldier:
                objectToSpawn = soldierPrefab;
                listToAddTo = soldierObjects;
                break;

            case Entity.cible:
                objectToSpawn = ciblePrefab;
                listToAddTo = cibleObjects;
                break;

            case Entity.trap:
                objectToSpawn = trapPrefab;
                listToAddTo = trapObjects;
                break;

            case Entity.none:
            default:
                objectToSpawn = null;
                listToAddTo = null;
                break;
        }


        if (objectToSpawn != null)
        {
            GameObject newObject = Instantiate(objectToSpawn, position, rotation, transform);
            if (newObject != null)
            {
                listToAddTo.Add(newObject);
                return newObject;
            }
        }

        return null;
    }

    public void BulletSpawned()
    {
        bulletsAlive++;
    }

    public void BulletDestroyed()
    {
        bulletsAlive--;
        if (bulletsAlive <= 0)
        {
            this.DelayedCall(delayToWaitAfterShot, delegate () {
                CibleManager.Instance.ShootingCompleted();
            });
            
        }
    }
}
