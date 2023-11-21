using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlot_StageEntity : UIBase
{
    public override bool Init()
    {
        return true;
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
