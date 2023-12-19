using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공용 재화 표시 UI를 사용하는 UIPopup의 경우 상속
/// </summary>
public class UIPopupMoney : UIPopup
{
    public override bool Init()
    {
        if (!base.Init())
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindEvent(GetButton((int)Buttons.Button_Back).gameObject, () => { Managers.UI.ClosePopupUI(this); });

        Managers.Event.AddVoidEvent(Define.VoidEventType.OnChangePlayerInfo, Redraw);
        Redraw();
        return true;
    }

    public void Redraw()
    {
        GetText((int)Texts.Text_PopupName).text = $"{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}";
        GetText((int)Texts.Text_Gem).text = $"{Managers.Game.playerData.gem}";
        GetText((int)Texts.Text_Gold).text = $"{Managers.Game.playerData.gold}";
    }

    private enum Buttons
    {
        Button_Back
    }

    private enum Texts
    {
        Text_PopupName, Text_Gem, Text_Gold
    }

    private void OnDisable()
    {
        Managers.Event.RemoveVoidEvent(Define.VoidEventType.OnChangePlayerInfo, Redraw);
    }
}
