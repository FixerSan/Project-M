using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_Gacha : UIPopup
{
    public override bool Init()
    {
        if (!base.Init())
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        BindEvent(GetButton((int)Buttons.Button_Back).gameObject, _callback: () => { Managers.UI.ClosePopupUI(this); });
        // BindEvent(GetButton((int)Buttons.Button_ShowChart).gameObject, _callback: OnClick_ShowChart);
        BindEvent(GetButton((int)Buttons.Button_TryGacha).gameObject, _callback: OnClick_TryGacha);
        BindEvent(GetButton((int)Buttons.Button_TryGachaTenTimes).gameObject, _callback: OnClick_TryGachaTenTimes);

        //GetText((int)Texts.Text_Confirmation).gameObject.SetActive(false);
        //GetText((int)Texts.Text_NotEnoughGem).gameObject.SetActive(false);

        RedrawUI();
        return true;
    }

    public void OnClick_TryGacha()
    {
        if (Managers.Gacha.TryGacha(1) == true)
        {
            Managers.UI.ShowPopupUI<UIPopup_GachaConfirmation>().Init(1, Managers.Gacha.OneGachaConsumeAmount);
        }
        else
            Debug.Log($"젬이 최소 {10}개 필요하다맨");
        //GetText((int)Texts.Text_Confirmation).gameObject.SetActive(true);
    }

    public void OnClick_TryGachaTenTimes()
    {
        if (Managers.Gacha.TryGacha(10) == true)
        {
            Managers.UI.ShowPopupUI<UIPopup_GachaConfirmation>().Init(10, 10 * Managers.Gacha.OneGachaConsumeAmount);
        }
        else
            Debug.Log($"젬이 최소 {100}개 필요하다맨");
        //GetText((int)Texts.Text_Confirmation).gameObject.SetActive(true);
    }

    public void OnClick_ShowChart()
    {
        Managers.UI.ShowPopupUI<UIPopup_GachaChart>();
    }


    public override void RedrawUI()
    {
        GetText((int)Texts.Text_UserGem).text = $"{Managers.Game.playerData.gem}";
        GetText((int)Texts.Text_UserGold).text = $"{Managers.Game.playerData.gold}";
    }

    private enum Buttons
    {
        Button_Back, Button_ShowChart, Button_TryGacha, Button_TryGachaTenTimes
    }

    private enum Texts
    {
        Text_Confirmation, Text_NotEnoughGem,
        Text_UserGem, Text_UserGold
    }
}
