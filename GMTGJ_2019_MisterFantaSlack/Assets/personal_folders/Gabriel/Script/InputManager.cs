using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public enum EDirection { Up, Down, Left, Right };

public class InputManager : MonoBehaviour
{
	public bool bInputBlock = false;

	public int NbCharacterActionComplete = 0;

	public UnityEvent ShootTrigger = new UnityEvent();
	public DirectionEvent MoveTrigger = new DirectionEvent();


	

	// SINGLETON

	public static InputManager Instance = null;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	// Update is called once per frae
	void Update()
    {
		//	cooldown for the action ???
		if (true)
		{
			if (Input.GetKeyDown(KeyCode.P))
			{
				StartPause();
			}
			else if (Input.GetKeyDown(KeyCode.R))
			{
				StartReset();
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				StartFire();
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				StartMove(EDirection.Up);
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				StartMove(EDirection.Down);
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				StartMove(EDirection.Left);
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				StartMove(EDirection.Right);
			} 
		}
	}

	//	cooldown for when the action is finish
	public void CharacterFinish()
	{
		NbCharacterActionComplete++;

		if(NbCharacterActionComplete >= 3)
		{
			bInputBlock = false;
			NbCharacterActionComplete = 0;
		}
	}


	//		INPUT FUNCTION


	//	Call to open the menu to go in the level selection or other shit like that
	public void StartPause()
	{
		#if (UNITY_EDITOR)
			Debug.Log("InputManager :: PAUSE");
		#endif

		GameManager.Instance.ReturnToHome();
	}

	//	Call to reset the curret level if the player fell stuck in the current state
	public void StartReset()
	{
		#if (UNITY_EDITOR)
			Debug.Log("InputManager :: RESET THE LEVEL");
		#endif

		GameManager.Instance.Restart();
	}

	//	Call to everyone to FIRAAAAAAAAAH
	public void StartFire()
	{
		#if (UNITY_EDITOR)
			Debug.Log("InputManager :: EVERY ONE, SHOOOOOOOOT");
		#endif

		if (ShootTrigger != null)
			ShootTrigger.Invoke();
	}

	//	Call each line from the direction specify to move in that direction
	//	if up, move the top row, the second row under, the third row under, etc
	//	it must be in that order to don't fuck up
	public void StartMove(EDirection MyDirection)
	{
		switch(MyDirection)
		{
			case EDirection.Up:
				#if (UNITY_EDITOR)
					Debug.Log("InputManager :: MOVE UP");
				#endif

				break;


			case EDirection.Down:
				#if (UNITY_EDITOR)
					Debug.Log("InputManager :: MOVE DOWN");
				#endif

				//	for each line
				for (int i = 0; i < 15; i++)
				{
					//	for each column
					for (int j = 0; j < 15; j++)
					{
						Vector2Int MyPosition = new Vector2Int(j, i);

						BaseCharacter MySoldier = IsSoldierValid(MyPosition);
						if(MySoldier != null)
						{
							Debug.Log("MOVE DOWN SOLDIER FOUND");
							GameTile TileVoisin = GameGrid.Instance.GetTileAtposition(MyPosition).GetTileRelativeToMe(EDirection.Down);
							if(TileVoisin != null)
							{
								Debug.Log("MOVE DOWN TILE VOISIN FOUND");
								if (TileVoisin.IsAccessible == true)
								{
									if (TileVoisin.IsSliding == false)
									{
										Vector3 MyDestination = MySoldier.gameObject.transform.position;//TileVoisin.Pos.ToVector3Int();
										MyDestination.y -= 1.0f;
										Debug.Log("MOVE DOWN to " + MyDestination.ToString());
										MySoldier.GoInThatDirection(MyDestination, 1);

									}
								}
							}
						}
					}
				}

				break;


			case EDirection.Left:
				#if (UNITY_EDITOR)
					Debug.Log("InputManager :: MOVE LEFT");
				#endif
				break;


			case EDirection.Right:
				#if (UNITY_EDITOR)
					Debug.Log("InputManager :: MOVE RIGHT");
				#endif
				break;


			default:
				#if (UNITY_EDITOR)
					Debug.Log("InputManager::StartMove() Something fuck up ???");
				#endif
				return;
		}

		//		Useless
		//if (MoveTrigger != null)
			//MoveTrigger.Invoke(MyDirection);
	}

	BaseCharacter IsSoldierValid(Vector2Int LocationOfActor)
	{
		BaseCharacter MySoldier = null;
		GameTile MyTile = GameGrid.Instance.GetTileAtposition(LocationOfActor);

		

		if (MyTile != null)
		{
			GameObject ObjectOnTile = MyTile.entityOnTop;
			if (ObjectOnTile != null)
			{
				MySoldier = ObjectOnTile.GetComponent<BaseCharacter>();
				if (MySoldier != null)
				{
					return MySoldier;
				}
			} 
		}

		return null;
	}


	/*void CheckIfMovePossible(Vector2Int LocationOfSoldier)
	{

	}*/
}
