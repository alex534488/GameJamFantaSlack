using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;

    public float speed = 2.5f;

    public float delayUntilDestroyed = 5;

    private Tween currentTween;

    public float destinationExtension = 10;

    private Vector3 bulletStartPosition;

    public GameObject shooter;

    public void SetDirectionAndStartPosition(Vector2 direction, Vector3 bulletStartPosition, GameObject shooter)
    {
        this.direction = direction;
        this.bulletStartPosition = bulletStartPosition;
        this.shooter = shooter;
}

    void Start()
    {
        GameManager.Instance.levelOver.AddListener(delegate() { Destroy(gameObject); });

        BeginMovement();
    }

    private void BeginMovement()
    {
        currentTween = transform.DOMove(bulletStartPosition + (direction * destinationExtension), speed);

        this.DelayedCall(delayUntilDestroyed,delegate { DestructBullet(); });
    }

    public void EntityCollision(GameObject otherObject)
    {
        if (otherObject == shooter)
            return;

        DestructibleObject destructibleObject = otherObject.GetComponent<DestructibleObject>();

        if(destructibleObject != null)
        {
            destructibleObject.DestructObject();
        }

        // DO SOLDIER SHOT HERE

        DestructBullet();
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
