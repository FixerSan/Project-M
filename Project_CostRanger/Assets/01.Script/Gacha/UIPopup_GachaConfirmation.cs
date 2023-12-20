using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_GachaConfirmation : UIPopup
{
    public bool Init(int _tryCount, int _consumeCount)
    {
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetText((int)Texts.Text_GachaCount).text = $"{_tryCount}회 모집";
        GetText((int)Texts.Text_ConsumeCount).text = $"{_consumeCount}";

        BindEvent(GetButton((int)Buttons.Button_Back).gameObject, _callback: OnClick_Back);
        BindEvent(GetButton((int)Buttons.Button_BackBG).gameObject, _callback: OnClick_Back);
        BindEvent(GetButton((int)Buttons.Button_Confirm).gameObject, _callback: () => { OnClick_Confirm(_tryCount); });

        return true;
    }

    public void OnClick_Back()
    {
        Managers.UI.ClosePopupUI(this);
    }

    public void OnClick_Confirm(int _tryCount)
    {
        Managers.UI.ClosePopupUI(this);
        Managers.Gacha.StartGachaAll(_tryCount);
        Managers.UI.ShowPopupUI<UIPopup_GachaAnimation>();
    }

    private enum Buttons
    {
        Button_Back, Button_Confirm, Button_BackBG
    }

    private enum Texts
    {
        Text_GachaCount, Text_ConsumeCount
    }
}
