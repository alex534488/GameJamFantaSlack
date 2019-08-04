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
				//	for each line
				for (int i = (GameGrid.Instance.width - 1); i >= 0; i--)
				{
					//	for each column
					for (int j = 0; j < GameGrid.Instance.height; j++)
					{
						Vector2Int MyPosition = new Vector2Int(j, i);
						BaseCharacter MySoldier = IsSoldierValid(MyPosition);

						if (MySoldier != null)
						{
							MoveSoldierToYourDirection(MyPosition, MySoldier, MyDirection);
						}
					}
				}
				break;


			case EDirection.Down:
				#if (UNITY_EDITOR)
					Debug.Log("InputManager :: MOVE DOWN");
				#endif

				//	for each line
				for (int i = 0; i < GameGrid.Instance.width; i++)
				{
					//	for each column
					for (int j = 0; j < GameGrid.Instance.height; j++)
					{
						Vector2Int MyPosition = new Vector2Int(j, i);

						BaseCharacter MySoldier = IsSoldierValid(MyPosition);
						if(MySoldier != null)
						{
							MoveSoldierToYourDirection(MyPosition, MySoldier, MyDirection);
						}
					}
				}
				break;


			case EDirection.Left:
				#if (UNITY_EDITOR)
					Debug.Log("InputManager :: MOVE LEFT");
				#endif
				//	for each column
				for (int i = 0; i < GameGrid.Instance.width; i++)
				{
					//	for each line
					for (int j = 0; j < GameGrid.Instance.height; j++)
					{
						Vector2Int MyPosition = new Vector2Int(i, j);
						BaseCharacter MySoldier = IsSoldierValid(MyPosition);

						if (MySoldier != null)
						{
							MoveSoldierToYourDirection(MyPosition, MySoldier, MyDirection);
						}
					}
				}
				break;


			case EDirection.Right:
				#if (UNITY_EDITOR)
					Debug.Log("InputManager :: MOVE RIGHT");
				#endif
				//	for each column
				for (int i = (GameGrid.Instance.width - 1); i >= 0; i--)
				{
					//	for each line
					for (int j = 0; j < GameGrid.Instance.height; j++)
					{
						Vector2Int MyPosition = new Vector2Int(i, j);
						BaseCharacter MySoldier = IsSoldierValid(MyPosition);

						if (MySoldier != null)
						{
							MoveSoldierToYourDirection(MyPosition, MySoldier, MyDirection);
						}
					}
				}
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

	//	la seconde etape du deplacement
	void MoveSoldierToYourDirection(Vector2Int MyPosition, BaseCharacter MySoldier, EDirection MyDirection)
	{
		Debug.Log("MOVE SOLDIER FOUND");
		GameTile MyTile = GameGrid.Instance.GetTileAtposition(MyPosition);
		GameTile TileVoisin = GameGrid.Instance.GetTileAtposition(MyPosition).GetTileRelativeToMe(MyDirection);
		if (TileVoisin != null)
		{
			Debug.Log("MOVE TILE VOISIN FOUND");
			if (TileVoisin.IsAccessible == true)
			{
				if (TileVoisin.IsSliding == false)
				{
					Vector3 MyDestination = GameGrid.GetCenterCellPosition(TileVoisin);
					TileVoisin.entityOnTop = MySoldier.gameObject;
					TileVoisin.IsAccessible = false;
					MyTile.entityOnTop = null;
					MyTile.IsAccessible = true;

					MySoldier.GoInThatDirection(MyDestination, 1);
				}
				else
				{
					//sliding
				}
			}
		}
	}
}
