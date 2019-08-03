using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;

    public float speed = 2.5f;

    private Tween currentTween;

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    void Start()
    {
        SetDirection(new Vector2(1,0));
        BeginMovement();
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
            if (currentTween != null)
            {
                currentTween.Kill();
            }

            Destroy(gameObject);
        }
    }
}
