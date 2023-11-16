using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_WorldMap_ChapterOne : UIPopup
{
    public override bool Init()
    {
        if(!base.Init()) return false;

        BindButton(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.Button_Close).gameObject, () => { Managers.UI.ClosePopupUI(this); });
        BindEvent(GetButton((int)Buttons.Button_StageOne).gameObject, () => 
        { 
            Managers.Game.StartPrepare(0);
            Managers.UI.ShowPopupUI<UIPopup_PrepareStage>();
            Debug.Log("눌리긴 했음");
        });

        return true;
    }

    private enum Buttons
    {
        Button_StageOne, Button_StageTwo, Button_Close
    }
}
