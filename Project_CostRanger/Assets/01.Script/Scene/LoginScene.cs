using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
    public override void Init(Action _callback)
    {
        Debug.Log("�α��� �� �ε��");
        _callback?.Invoke();
    }

    public override void Clear()
    {

    }


    public override void SceneEvent(int _eventIndex, Action _callback = null)
    {

    }
}
