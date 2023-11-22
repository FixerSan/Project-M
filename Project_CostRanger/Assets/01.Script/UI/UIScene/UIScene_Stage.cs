using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIScene_Stage : UIScene
{
    public override bool Init()
    {
        if(!base.Init()) return false;
        BindText(typeof(Texts));

        Managers.Event.AddVoidEvent(RedrawUI);
        return true;
    }

    public void RedrawUI(Define.VoidEventType _eventType)
    {
        if (_eventType != Define.VoidEventType.OnChangeBattle) return;

        GetText((int)Texts.Text_Timer).text = $"{(int)Managers.Game.battleStageSystem.time}";
    }

    public void OnDisable()
    {
        Managers.Event.RemoveVoidEvent(RedrawUI);
    }

    private enum Texts
    {
        Text_Timer
    }
}
