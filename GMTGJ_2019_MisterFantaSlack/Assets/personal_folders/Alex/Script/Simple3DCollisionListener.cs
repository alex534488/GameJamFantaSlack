using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Simple3DCollisionListener : MonoBehaviour
{
    [System.Serializable] public class EntityCollisionEvent : UnityEvent<GameObject> { }


    public EntityCollisionEvent onTriggerEnter = new EntityCollisionEvent();
    public EntityCollisionEvent onTriggerExit = new EntityCollisionEvent();

    public EntityCollisionEvent onCollisionEnter = new EntityCollisionEvent();
    public EntityCollisionEvent onCollisionExit = new EntityCollisionEvent();

    private List<GameObject> objectInside = new List<GameObject>();

    void OnTriggerEnter(Collider collision)
    {
        if (onTriggerEnter != null)
        {
            onTriggerEnter.Invoke(collision.gameObject);
        }

        objectInside.Add(collision.gameObject);
    }

    void OnTriggerExit(Collider collision)
    {
        if (onTriggerExit != null)
        {
            onTriggerEnter.Invoke(collision.gameObject);
        }

        objectInside.Remove(collision.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (onCollisionEnter != null)
        {
            onCollisionEnter.Invoke(collision.collider.gameObject);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (onCollisionExit != null)
        {
            onCollisionExit.Invoke(collision.collider.gameObject);
        }
    }

    public List<GameObject> GetAllEntitiesInsideCollider()
    {
        return objectInside;
    }
}
