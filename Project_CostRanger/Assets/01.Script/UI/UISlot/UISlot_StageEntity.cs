using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlot_StageRanger : UIBase
{
    private RangerController controller;

    public override bool Init()
    {
        if(!base.Init())    return false;
        return true;
    }

    public void Init(RangerController _controller)
    {

    }

    public void Redraw()
    {

    }


    private enum Images
    {
        Image_Cooltime, Image_Illust, Image_HP
    }
    private enum Texts
    {
        Text_Cooltime
    }
}
