﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public virtual void DestructObject()
    {
        gameObject.SetActive(false);
    }
}