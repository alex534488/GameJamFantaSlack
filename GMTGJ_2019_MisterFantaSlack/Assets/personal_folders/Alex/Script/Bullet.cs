using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;

    public float speed = 2.5f;

    private Tween currentTween;

    public Vector2 lowerLeftLimit;
    public Vector2 upperRightLimit;

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    void Start()
    {
        GameManager.Instance.levelOver.AddListener(delegate() { Destroy(gameObject); });

        BeginMovement();
    }

    void Update()
    {
        // out of bounds
        Vector3 pos = transform.position;
        if ((pos.x >= upperRightLimit.x || pos.x <= lowerLeftLimit.x) && 
            pos.y >= upperRightLimit.y || pos.y <= lowerLeftLimit.y)
        {
            DestructBullet();
        }
    }

    private void BeginMovement()
    {
        currentTween = transform.DOMove(direction * 10, speed);
    }

    public void EntityCollision(GameObject otherObject)
    {
        DestructibleObject destructibleObject = otherObject.GetComponent<DestructibleObject>();

        bool hasHitSomething = false;

        if(destructibleObject != null)
        {
            destructibleObject.DestructObject();
            hasHitSomething = true;
        }

        // DO SOLDIER SHOT HERE
        if (hasHitSomething)
        {
            DestructBullet();
        }
    }

    private void DestructBullet()
    {
        if (currentTween != null)
        {
            currentTween.Kill();
        }

        EntitySpawner.Instance.BulletDestroyed();

        Destroy(gameObject);
    }
}
