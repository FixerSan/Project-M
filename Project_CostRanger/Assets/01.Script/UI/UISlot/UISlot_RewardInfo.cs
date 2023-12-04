using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlot_RewardInfo : UIBase
{
    public override bool Init()
    {
        if (!base.Init()) return false;
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetImage((int)Images.Image_RewardCount).gameObject.SetActive(false);
        return true;
    }

    public void DrawSlot(Define.RewardType _type, int _value)
    {
        isDrawed = true;
        GetImage((int)Images.Image_RewardCount).gameObject.SetActive(false);
        Managers.Resource.Load<Sprite>(_type.ToString(), (_sprite) => { GetImage((int)Images.Image_Illust).sprite = _sprite; });
        if(_value != 0)
        {
            GetImage((int)Images.Image_RewardCount).gameObject.SetActive(true);
            GetText((int)Texts.Text_RewardCount).text = _value.ToString();
        }
    }

    private enum Images
    {
        Image_Illust, Image_RewardCount
    }


    private enum Texts
    {
        Text_RewardCount
    }
}
