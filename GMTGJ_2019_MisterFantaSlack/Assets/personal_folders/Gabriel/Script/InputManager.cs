using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	enum EDirection { Up, Down, Left, Right };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//	cooldown for the action ???
		if (true)
		{
			if(Input.GetKeyDown(KeyCode.R))
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

	//	Call to reset the curret level if the player fell stuck in the current state
	void StartReset()
	{
		#if (UNITY_EDITOR)
			Debug.Log("InputManager :: RESET THE LEVEL");
		#endif
	}

	//	Call to everyone to FIRAAAAAAAAAH
	void StartFire()
	{
		#if (UNITY_EDITOR)
			Debug.Log("InputManager :: EVERY ONE, SHOOOOOOOOT");
		#endif
	}

	//	Call each line from the direction specify to move in that direction
	//	if up, move the top row, the second row under, the third row under, etc
	//	it must be in that order to don't fuck up
	void StartMove(EDirection MyDirection)
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
				break;
		}
	}
}
