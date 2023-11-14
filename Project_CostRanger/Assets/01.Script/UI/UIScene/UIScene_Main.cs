using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScene_Main : UIScene
{
    public override bool Init()
    {
        if (!base.Init())
            return false;

        BindButton(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.Button_Battle).gameObject, () => { Managers.UI.ShowPopupUI<UIPopup_WorldMap_ChapterOne>(); });


        return true;
    }

    private enum Buttons
    {
        Button_Battle
    }
}
