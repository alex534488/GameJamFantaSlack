using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cible : DestructibleObject
{
    public override void DestructObject()
    {
        base.DestructObject();

        if(CibleManager.Instance != null)
        {
            CibleManager.Instance.TargetShotted();
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
    }
}
