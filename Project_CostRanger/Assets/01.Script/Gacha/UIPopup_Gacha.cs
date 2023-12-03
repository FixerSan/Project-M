using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_Gacha : UIPopup
{
    // 가챠 시스템 연결

    public override bool Init()
    {
        if (!base.Init())
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        BindEvent(GetButton((int)Buttons.Button_Back).gameObject, _callback: () => { Managers.UI.ClosePopupUI(this); });
        BindEvent(GetButton((int)Buttons.Button_TryGacha).gameObject, _callback: OnClick_TryGacha);
        BindEvent(GetButton((int)Buttons.Button_TryGachaTenTimes).gameObject, _callback: OnClick_TryGachaTenTimes);
        
        GetText((int)Texts.Text_Confirmation).gameObject.SetActive(false);
        GetText((int)Texts.Text_NotEnoughGem).gameObject.SetActive(false);
        return true;
    }

    public void OnClick_TryGacha()
    {
        if (Managers.Gacha.TryGacha(1) == true)
            Managers.Gacha.TryGacha(1);
            //GetText((int)Texts.Text_Confirmation).gameObject.SetActive(true);
    }

    public void OnClick_TryGachaTenTimes()
    {
        if (Managers.Gacha.TryGacha(10) == true)
            Managers.Gacha.TryGacha(10);
            //GetText((int)Texts.Text_Confirmation).gameObject.SetActive(true);
    }

    private enum Buttons
    {
        Button_Back, Button_TryGacha, Button_TryGachaTenTimes
    }

    private enum Texts
    {
        Text_Confirmation, Text_NotEnoughGem
    }
}
