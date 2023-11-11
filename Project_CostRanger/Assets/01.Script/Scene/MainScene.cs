using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene
{
    public override void Init(Action _callback)
    {
        Managers.UI.ShowSceneUI<UIScene_Main>();
        Managers.Game.state = Define.GameState.BattleBefore;
    }

    public override void Clear()
    {

    }


    public override void SceneEvent(int _eventIndex, Action _callback = null)
    {

    }
}
