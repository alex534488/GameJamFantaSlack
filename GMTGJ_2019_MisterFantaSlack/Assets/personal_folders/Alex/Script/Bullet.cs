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

    private GameObject shooter;

    public void SetDirectionAndStartPosition(Vector2 direction, Vector3 bulletStartPosition, GameObject shooter)
    {
        this.direction = direction;
        this.bulletStartPosition = bulletStartPosition;
        this.shooter = shooter;
    }

    void Start()
    {
        GameManager.Instance.levelOver.AddListener(delegate () { Destroy(gameObject); });

        BeginMovement();
    }

    private void BeginMovement()
    {
        currentTween = transform.DOMove(bulletStartPosition + (direction * destinationExtension), destinationExtension / speed);
    }

    void Update()
    {
        Vector3 currentBulletPos = transform.position;

        if (currentBulletPos.x > GameGrid.Instance.maxBounds.x ||
            currentBulletPos.x < GameGrid.Instance.minBounds.x ||
            currentBulletPos.y > GameGrid.Instance.maxBounds.y ||
            currentBulletPos.y < GameGrid.Instance.minBounds.y)
        {
            DestructBullet();
        }
    }

    public void EntityCollision(GameObject otherObject)
    {
        if (otherObject == shooter)
            return;

        bool hasHit = false;

        DestructibleObject destructibleObject = otherObject.GetComponent<DestructibleObject>();

        if (destructibleObject != null)
        {
            destructibleObject.DestructObject();

            hasHit = true;
        }

        BaseCharacter character = otherObject.GetComponent<BaseCharacter>();

        if (character != null)
        {
            character.OnHit();

            hasHit = true;
        }

        BulletBlocker bulletBlocker = otherObject.GetComponent<BulletBlocker>();

        if (bulletBlocker != null)
        {
            hasHit = true;
        }

        if (hasHit)
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
