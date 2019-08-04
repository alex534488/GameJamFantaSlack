using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;

public enum EDirection { Up, Down, Left, Right };

public class InputManager : MonoBehaviour
{
    // SINGLETON

    public static InputManager Instance = null;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        GameManager.Instance.gameStarted.AddListener(delegate () { inputBlocked = false; });
        GameManager.Instance.levelOver.AddListener(delegate () { inputBlocked = true; });
    }

    // EVENTS

    public UnityEvent ShootTrigger = new UnityEvent();

    // DATA

    private bool inputBlocked = true;

    private int amountOfCharacters;
    private int characterLeftToMove;

    // Init
    public void SetAmountOfCharacters(int amount)
    {
        amountOfCharacters = amount;
        characterLeftToMove = amountOfCharacters;
        InputReactivated();
    }

    void Update()
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
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            StartMove(EDirection.Up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            StartMove(EDirection.Down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            StartMove(EDirection.Left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            StartMove(EDirection.Right);
        }
    }

    public void PlayerCompletedItsMovement()
    {
        characterLeftToMove--;

        if(characterLeftToMove <= 0)
        {
            characterLeftToMove = amountOfCharacters;
            InputReactivated();
        }
    }

    public void InputDeactivated()
    {
        inputBlocked = true;
    }

    public void InputReactivated()
    {
        if (GameManager.Instance.levelCompleted)
            return;

        inputBlocked = false;

        // Resolve deaths
        List<GameObject> soldiers = EntitySpawner.Instance.soldierObjects;
        for (int i = 0; i < soldiers.Count; i++)
        {
            BaseCharacter character = soldiers[i].GetComponent<BaseCharacter>();

            if (character != null)
            {
                character.OnDeath();
            }
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
        if (inputBlocked)
            return;
        KimAnimation.Instance.AnimationAngryKim();
        GameManager.Instance.ui.kimBubble.Say("Fire!", true, 0.5f);

        InputDeactivated();

        #if (UNITY_EDITOR)
        Debug.Log("InputManager :: EVERY ONE, SHOOOOOOOOT");
        #endif

        if (ShootTrigger != null)
            ShootTrigger.Invoke();

        // Wait until shooting is done to enable inputs
        EntitySpawner.Instance.AllBulletsDestroyed.AddListener(delegate () { InputReactivated(); });
    }

    //	Call each line from the direction specify to move in that direction
    //	if up, move the top row, the second row under, the third row under, etc
    //	it must be in that order to don't fuck up
    public void StartMove(EDirection MyDirection)
    {
        if (inputBlocked)
            return;

        GameManager.Instance.ui.kimBubble.Say("Move!", true, 0.5f);

        InputDeactivated();

        int amountOfSoldierFound = 0;

        switch (MyDirection)
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
                            amountOfSoldierFound++;
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
                            amountOfSoldierFound++;
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
                            amountOfSoldierFound++;
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
                            amountOfSoldierFound++;
                            MoveSoldierToYourDirection(MyPosition, MySoldier, MyDirection, 0);
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

        if(amountOfSoldierFound < EntitySpawner.Instance.soldierObjects.Count)
        {
            Debug.LogError("Error, not all soldiers found in the map");
        }
    }

    private BaseCharacter IsSoldierValid(Vector2Int LocationOfActor)
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

    //	Apply Displacement
    private void MoveSoldierToYourDirection(Vector2Int MyPosition, BaseCharacter MySoldier, EDirection MyDirection, int movementsInARow = 0)
    {
        Debug.Log("MOVE SOLDIER FOUND");
        GameTile MyTile = GameGrid.Instance.GetTileAtposition(MyPosition);
        GameTile TileVoisin = GameGrid.Instance.GetTileAtposition(MyPosition).GetTileRelativeToMe(MyDirection);
        if (TileVoisin != null)
        {
            Debug.Log("MOVE TILE VOISIN FOUND");
            if (TileVoisin.IsAccessible)
            {
                List<Action> actionQueueToExecute = new List<Action>();

                actionQueueToExecute.Add(delegate () {
                    GameGrid.Instance.AddMyselfOnTile(MySoldier.gameObject);
                });

                if (TileVoisin.entityOnTop != null)
                {
                    BaseCharacter nearCharacter = TileVoisin.entityOnTop.GetComponent<BaseCharacter>();
                    if (nearCharacter != null)
                    {
                        InputManager.Instance.PlayerCompletedItsMovement();
                        return;
                    }

                    Trap trap = TileVoisin.entityOnTop.GetComponent<Trap>();
                    if (trap != null)
                    {
                        actionQueueToExecute.Add(delegate () 
                        {
                            MySoldier.OnHit();
                            trap.DestructObject();
                        });
                    }
                }

                if (TileVoisin.IsRotating)
                {
                    actionQueueToExecute.Add(delegate () { MySoldier.RotateCharacter(); });
                }

                if (TileVoisin.IsSliding)
                {
                    actionQueueToExecute.Add(delegate () {
                        Vector3 soldierPos = MySoldier.transform.position;
                        movementsInARow++;
                        MoveSoldierToYourDirection(TileVoisin.Pos, MySoldier,MyDirection, movementsInARow);
                    });
                }
                else
                {
                    actionQueueToExecute.Add(delegate () { InputManager.Instance.PlayerCompletedItsMovement(); });
                }

                GameGrid.Instance.RemoveMyselfFromTile(MySoldier.gameObject);

                Vector3 MyDestination = GameGrid.GetCenterCellPosition(TileVoisin);
                MySoldier.GoInThatDirection(MyDestination, 1, movementsInARow: movementsInARow, actionQueueToExecute);
            }
            else
            {
                InputManager.Instance.PlayerCompletedItsMovement();
            }
        }
        else
        {
            InputManager.Instance.PlayerCompletedItsMovement();
        }
    }
}
