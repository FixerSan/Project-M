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
        BindButton(typeof(Buttons));

        BindEvent(GetButton((int)Buttons.Button_FastSpeed).gameObject, OnClick_FastSpeed);
        BindEvent(GetButton((int)Buttons.Button_AutoSkill).gameObject, OnClick_AutoSkill);

        Managers.Event.AddVoidEvent(Define.VoidEventType.OnChangeBattle ,RedrawUI);
        return true;
    }

    public void RedrawUI()
    {
        GetText((int)Texts.Text_Timer).text = $"{(int)Managers.Game.battleStageSystem.time}";
        GetButton((int)Buttons.Button_FastSpeed).interactable = !Managers.Game.battleStageSystem.isFastSpeed;
        GetButton((int)Buttons.Button_AutoSkill).interactable = !Managers.Game.battleStageSystem.isAutoSkill;
    }
    public void OnClick_FastSpeed()
    {
        Managers.Game.battleStageSystem.SetFastSpeed(!Managers.Game.battleStageSystem.isFastSpeed);
    }

    public void OnClick_AutoSkill()
    {
        Managers.Game.battleStageSystem.SetAutoSkill(!Managers.Game.battleStageSystem.isAutoSkill);
    }


    public void OnDisable()
    {
        Managers.Event.RemoveVoidEvent(Define.VoidEventType.OnChangeBattle, RedrawUI);
    }

    private enum Texts
    {
        Text_Timer
    }

    private enum Buttons
    {
        Button_FastSpeed, Button_AutoSkill
    }
}
