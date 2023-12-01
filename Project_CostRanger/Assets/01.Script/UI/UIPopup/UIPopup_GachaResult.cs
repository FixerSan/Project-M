using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_GachaResult : UIPopup
{
    public UISlot_GachaInfo[] gachaInfoSlots;

    public override bool Init()
    {
        if (!base.Init())
            return base.Init();

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));


        return true;
    }


    public enum Buttons
    {

    }

    public enum Texts
    {

    }
}
