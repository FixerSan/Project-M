using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
    public override void Init(Action _callback)
    {
        Managers.UI.ShowSceneUI<UIScene_Login>();
        _callback?.Invoke();
    }

    public override void Clear()
    {

    }


    public override void SceneEvent(int _eventIndex, Action _callback = null)
    {

    }
}
