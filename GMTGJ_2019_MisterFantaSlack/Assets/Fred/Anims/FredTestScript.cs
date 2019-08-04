using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FredTestScript : MonoBehaviour
{
    private void Update()
    {
        Vector3 move = Vector3.zero;
        SoldierAnimator.Direction dir = SoldierAnimator.Direction.left;
        float duration = 0.5f;

        if (Input.GetKeyDown(KeyCode.D))
        {
            GetComponent<SoldierAnimator>().PlayDeathAnimation(() => Debug.Log("Death complete"));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<SoldierAnimator>().PlayFireAnimation();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            move = Vector3.right;
            dir = SoldierAnimator.Direction.right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move = Vector3.left;
            dir = SoldierAnimator.Direction.left;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            move = Vector3.up;
            dir = SoldierAnimator.Direction.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            move = Vector3.down;
            dir = SoldierAnimator.Direction.down;
        }



        if (move != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                move *= 3;
                duration *= 3;
            }
            GetComponent<SoldierAnimator>().PlayWalkAnimation(dir);
            transform.DOMove(move, duration).SetRelative();
        }
    }
}
