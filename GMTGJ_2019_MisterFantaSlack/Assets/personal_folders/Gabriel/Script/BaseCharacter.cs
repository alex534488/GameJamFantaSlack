using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
	public EDirection CurrentDirectionFacing = EDirection.Up;
	public bool IsDeath = false;

	// Start is called before the first frame update
	void Start()
	{
		UpdateDirectionFacingForRotation();
		InputManager.Instance.ShootTrigger.AddListener(ShootFacingDirection);
		InputManager.Instance.MoveTrigger.AddListener(GoInThatDirection);
	}

	// Update is called once per frame
	void Update()
	{

		if(Input.GetKeyDown(KeyCode.T))
		{
			RotateCharacter();
		}
		else if (Input.GetKeyDown(KeyCode.H))
		{
			OnHit();
		}
		else if(Input.GetKeyDown(KeyCode.K))
		{
			OnDeath();
		}
	}

	////////////////////////////////////////////////////////////////
	//	SHOOT FUNCTION

	public void ShootFacingDirection()
	{
		#if (UNITY_EDITOR)
		Debug.Log("On Shoot with Character : " + gameObject.name);
		#endif
	}



	////////////////////////////////////////////////////////////////
	//	MOVE FUNCTION

	public void GoInThatDirection(EDirection Direction)
	{
		#if (UNITY_EDITOR)
		Debug.Log("Go in the Direction :" + Direction.ToString() + " With the Character : " + gameObject.name);
		#endif
	}



	////////////////////////////////////////////////////////////////
	//	DEATH FUNCTION	


	//	Ce personnage c'est fait tirer dessus, s'il était deja mort, retourne false
	//	si ce tir la bien tuer, retourne true
	public bool OnHit()
	{
		if(IsDeath == false)
		{
			IsDeath = true;
			return true;
		}
		return false;
	}

	//	si ce personnage c'est fait tirer dessus, cette methode declanche l'anim et 
	//	tout les truc qui doit gerer la mort pour finir par le detruit
	//	S'il n'est pas blesser, elle fait rien
	public void OnDeath()
	{
		if (IsDeath == false)
			return;

		#if (UNITY_EDITOR)
			Debug.Log("The character : " + gameObject.name + " is death");
#endif

		//	clear la tuile ou il se trouve

		//	play the death animation

		//	tout est fini, call action complete au input manager

		InputManager.Instance.CharacterFinish();

		//	destroy

		gameObject.Destroy();
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
