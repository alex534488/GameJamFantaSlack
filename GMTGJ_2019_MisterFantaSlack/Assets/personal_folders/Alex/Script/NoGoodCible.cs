using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGoodCible : DestructibleObject
{
    public override void DestructObject()
    {
        base.DestructObject();

        GameManager.Instance.Restart();
    }
}
