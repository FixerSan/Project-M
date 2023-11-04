using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_WorldMap_ChapterOne : UIPopup
{
    public override bool Init()
    {
        if(!base.Init()) return false;

        BindButton(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.Button_StageOne_One).gameObject, () => 
        {

            Managers.Game.battleInfo.SetStageData(1001);
            Managers.UI.ShowPopupUI<UIPopup_BattleBefore>(); 
        });
        BindEvent(GetButton((int)Buttons.Button_StageOne_Two).gameObject, () =>
        {

            Managers.Game.battleInfo.SetStageData(1002);
            Managers.UI.ShowPopupUI<UIPopup_BattleBefore>();
        });
        BindEvent(GetButton((int)Buttons.Button_ClosePopup).gameObject, () => { Managers.UI.ClosePopupUI(this); });

        return true;
    }

    private enum Buttons
    {
        Button_StageOne_One, Button_StageOne_Two, Button_ClosePopup
    }
}
