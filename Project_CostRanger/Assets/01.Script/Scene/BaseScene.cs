using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseScene : MonoBehaviour
{
    public abstract void Init(Action _callback = null);
    public abstract void Clear();
    public abstract void SceneEvent(int _eventIndex, Action _callback = null);
}
