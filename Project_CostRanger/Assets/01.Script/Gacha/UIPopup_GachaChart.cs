using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_GachaChart : UIPopup
{
    public override bool Init()
    {
        if (!base.Init())
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        BindEvent(GetButton((int)Buttons.Button_Back).gameObject, _callback: () => { Managers.UI.ClosePopupUI(this); });
        BindEvent(GetButton((int)Buttons.Button_BackBG).gameObject, _callback: () => { Managers.UI.ClosePopupUI(this); });

        return true;
    }

    private void OnEnable()
    {
        RedrawChartInfos();
    }

    public void RedrawChartInfos()
    {
        Debug.Log("아오 슬롯시치");
        // GachaSystem의 currentTable 값을 불러옴
        // for문으로 각 slot의 값을 동적 생성하고 Redraw
    }

    private enum Buttons
    {
        Button_Back, Button_BackBG
    }

    private enum Texts
    {

    }
}
