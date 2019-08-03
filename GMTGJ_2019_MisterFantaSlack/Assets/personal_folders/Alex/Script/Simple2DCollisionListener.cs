using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Simple2DCollisionListener : MonoBehaviour
{
    [System.Serializable] public class EntityCollisionEvent : UnityEvent<GameObject>{ }

    public EntityCollisionEvent onTriggerEnter = new EntityCollisionEvent();
    public EntityCollisionEvent onTriggerExit = new EntityCollisionEvent();

    public EntityCollisionEvent onCollisionEnter = new EntityCollisionEvent();
    public EntityCollisionEvent onCollisionExit = new EntityCollisionEvent();

    private List<GameObject> objectInside = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(onTriggerEnter != null)
        {
            onTriggerEnter.Invoke(collision.gameObject);
        }

        objectInside.Add(collision.gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(onTriggerExit != null)
        {
            onTriggerEnter.Invoke(collision.gameObject);
        }

        objectInside.Remove(collision.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(onCollisionEnter != null)
        {
            onCollisionEnter.Invoke(collision.otherCollider.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(onCollisionExit != null)
        {
            onCollisionExit.Invoke(collision.otherCollider.gameObject);
        }
    }

    public List<GameObject> GetAllEntitiesInsideCollider()
    {
        return objectInside;
    }
}
