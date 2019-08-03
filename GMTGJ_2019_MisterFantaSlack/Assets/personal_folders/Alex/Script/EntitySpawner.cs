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

        switch (entity)
        {
            case Entity.none:
                return null;
            case Entity.destructible:
                objectToSpawn = destructiblePrefab;
                if (objectToSpawn != null)
                {
                    GameObject newObject = Instantiate(objectToSpawn, position, rotation, transform);
                    if(newObject != null)
                    {
                        destructibleObjects.Add(newObject);
                    }
                }
                return objectToSpawn;
            case Entity.soldier:
                objectToSpawn = soldierPrefab;
                if (objectToSpawn != null)
                {
                    GameObject newObject = Instantiate(objectToSpawn, position, rotation, transform);
                    if (newObject != null)
                    {
                        soldierObjects.Add(newObject);
                    }
                }
                return objectToSpawn;
            case Entity.cible:
                objectToSpawn = ciblePrefab;
                if (objectToSpawn != null)
                {
                    GameObject newObject = Instantiate(objectToSpawn, position, rotation, transform);
                    if (newObject != null)
                    {
                        cibleObjects.Add(newObject);
                    }
                }
                return objectToSpawn;
            case Entity.trap:
                objectToSpawn = trapPrefab;
                if (objectToSpawn != null)
                {
                    GameObject newObject = Instantiate(objectToSpawn, position, rotation, transform);
                    if (newObject != null)
                    {
                        trapObjects.Add(newObject);
                    }
                }
                return objectToSpawn;
            default:
                return null;
        }
    }

    public void BulletSpawned()
    {
        bulletsAlive++;
    }

    public void BulletDestroyed()
    {
        bulletsAlive--;
        if(bulletsAlive <= 0)
        {
            CibleManager.Instance.ShootingCompleted();
        }
    }
}
