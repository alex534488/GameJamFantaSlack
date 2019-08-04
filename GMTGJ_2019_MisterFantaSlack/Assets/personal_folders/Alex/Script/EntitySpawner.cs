using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntitySpawner : MonoBehaviour
{
    // SINGLETON

    public static EntitySpawner Instance = null;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public UnityEvent AllBulletsDestroyed = new UnityEvent();

    public float delayToWaitAfterShot = 0.5f;

    void Start()
    {
        GameManager.Instance.gameStarted.AddListener(OnGameStart);
        GameManager.Instance.levelOver.AddListener(OnLevelEnd);
    }

    public void OnGameStart()
    {
        CibleManager.Instance.SetTargetAmountInLevel(cibleObjects.Count);
        InputManager.Instance.SetAmountOfCharacters(soldierObjects.Count);
    }

    public void OnLevelEnd()
    {
        DestroyAllInList(destructibleObjects);
        DestroyAllInList(soldierObjects);
        DestroyAllInList(cibleObjects);
        DestroyAllInList(trapObjects);
        DestroyAllInList(noGoodCibleObjects);
        DestroyAllInList(blockerObjects);
    }

    private void DestroyAllInList(List<GameObject> objs)
    {
        for (int i = 0; i < objs.Count; i++)
        {
            Destroy(objs[i]);
        }

        objs.Clear();
    }

    public enum Entity
    {
        none,
        destructible,
        soldier,
        cible,
        trap,
        noGoodCible,
        blocker
    }

    [HideInInspector]
    public List<GameObject> destructibleObjects = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> soldierObjects = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> cibleObjects = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> trapObjects = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> noGoodCibleObjects = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> blockerObjects = new List<GameObject>();

    public GameObject destructiblePrefab;
    public GameObject soldierPrefab;
    public GameObject ciblePrefab;
    public GameObject trapPrefab;
    public GameObject noGoodCiblePrefab;
    public GameObject blockerPrefab;

    private int bulletsAlive = 0;

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

            case Entity.noGoodCible:
                objectToSpawn = noGoodCiblePrefab;
                listToAddTo = noGoodCibleObjects;
                break;

            case Entity.blocker:
                objectToSpawn = blockerPrefab;
                listToAddTo = blockerObjects;
                break;

            case Entity.none:
                objectToSpawn = null;
                listToAddTo = null;
                break;

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

                if (AllBulletsDestroyed != null)
                {
                    AllBulletsDestroyed.Invoke();
                }
            });
            
        }
    }

    public void OnSoldierDeath(GameObject soldier)
    {
        soldierObjects.Remove(soldier);
        InputManager.Instance.SetAmountOfCharacters(soldierObjects.Count);
    }
}
