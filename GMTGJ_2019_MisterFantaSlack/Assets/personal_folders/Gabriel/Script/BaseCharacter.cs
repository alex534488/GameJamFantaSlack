using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BaseCharacter : MonoBehaviour
{
    // PUBLIC FIELDS

    public GameObject bulletPrefab;
    public GameObject bulletStartPosition;

    public float TimeToPassOneTile = 1.0f;

    // LOCAL DATA

    private Tween currentTween;

    private EDirection CurrentDirectionFacing = EDirection.Up;

	private bool isDying = false;
    private bool isDead = false;


	// Start is called before the first frame update
	void Start()
	{
        isDying = false;
        isDead = false;

        UpdateDirectionFacingForRotation();
		InputManager.Instance.ShootTrigger.AddListener(ShootFacingDirection);
	}

	////////////////////////////////////////////////////////////////
	//	SHOOT FUNCTION

	public void ShootFacingDirection()
	{
		#if (UNITY_EDITOR)
		Debug.Log("On Shoot with Character : " + gameObject.name);
    #endif

        Bullet bullet = Instantiate(bulletPrefab, bulletStartPosition.transform.position, Quaternion.identity, EntitySpawner.Instance.transform).GetComponent<Bullet>();

        EntitySpawner.Instance.BulletSpawned();

        switch (CurrentDirectionFacing)
        {
            case EDirection.Up:
                bullet.SetDirectionAndStartPosition(new Vector2(0, 1), bulletStartPosition.transform.position, gameObject);
                break;
            case EDirection.Down:
                bullet.SetDirectionAndStartPosition(new Vector2(0, -1), bulletStartPosition.transform.position, gameObject);
                break;
            case EDirection.Left:
                bullet.SetDirectionAndStartPosition(new Vector2(-1, 0), bulletStartPosition.transform.position, gameObject);
                break;
            case EDirection.Right:
                bullet.SetDirectionAndStartPosition(new Vector2(1,0), bulletStartPosition.transform.position, gameObject);
                break;
            default:
                break;
        }
    }



	////////////////////////////////////////////////////////////////
	//	MOVE FUNCTION

	public void GoInThatDirection(Vector3 Destination, int NbTile, List<Action> onTileReached = null)
	{
		float Duration = TimeToPassOneTile * NbTile;
        if(onTileReached == null)
        {
            currentTween = transform.DOMove(Destination, Duration).SetUpdate(true).OnComplete(delegate () { InputManager.Instance.PlayerCompletedItsMovement(); });
        }
        else
        {
            currentTween = transform.DOMove(Destination, Duration).SetUpdate(true).OnComplete(delegate () {
                foreach (Action action in onTileReached)
                {
                    if(action != null)
                        action();
                }
            });
        }
	}

	////////////////////////////////////////////////////////////////
	//	DEATH FUNCTION	

	// Character has been hit, should be death so we set his state as dead
	public void OnHit()
	{
        isDying = true;
    }

	//	Apply Death
	public void OnDeath()
	{
        // we're dying ?
        if (isDying)
        {
            if (isDead)
                return;

            isDead = true;

#if (UNITY_EDITOR)
            Debug.Log("The character : " + gameObject.name + " is death");
#endif

            EntitySpawner.Instance.OnSoldierDeath(gameObject);

            gameObject.Destroy();
        }
	}

	////////////////////////////////////////////////////////////////
	//	FACING DIRECTION FUNCTION	


	//	start the rotation of 90 degree of the character
	public void RotateCharacter()
	{
		switch (CurrentDirectionFacing)
		{
			case EDirection.Up:
				CurrentDirectionFacing = EDirection.Right;
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
				break;
			case EDirection.Down:
				CurrentDirectionFacing = EDirection.Left;
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
				break;
			case EDirection.Left:
				CurrentDirectionFacing = EDirection.Up;
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
				break;
			case EDirection.Right:
				CurrentDirectionFacing = EDirection.Down;
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
				break;
			default:
				break;
		}
	}

	//	Update the rotation based on the Current Direction Facing of the character
	void UpdateRotationForDirectionFacing()
	{
		switch (CurrentDirectionFacing)
		{
			case EDirection.Up:
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
				break;
			case EDirection.Down:
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
				break;
			case EDirection.Left:
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
				break;
			case EDirection.Right:
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
				break;
			default:
				break;
		}
	}

	//	Update the Current direction facing based on the rotation of the character
	void UpdateDirectionFacingForRotation()
	{
		int zValue = (int)transform.rotation.eulerAngles.z;

		if (zValue < 45 || zValue > 315)
		{
			CurrentDirectionFacing = EDirection.Right;
			return;
		}
		else if (zValue >= 45 && zValue <= 135)
		{
			CurrentDirectionFacing = EDirection.Up;
			return;
		}
		else if (zValue > 135 && zValue < 225)
		{
			CurrentDirectionFacing = EDirection.Left;
			return;
		}
		else if (zValue >= 225 && zValue <= 315)
		{
			CurrentDirectionFacing = EDirection.Down;
			return;
		}
	}

}
