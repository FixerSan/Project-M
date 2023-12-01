using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UISlot_GachaInfo : UIBase
{
    public void Init(int _uid, bool _isAlreadyObtained)
    {
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetObject((int)Images.Image_ObtainedStatusBG).SetActive(false);
        GetImage((int)Images.Image_RangerBG).sprite = Managers.Resource.Load<Sprite>($"{_uid}.sprite");

        if (!_isAlreadyObtained)
            GetObject((int)Images.Image_ObtainedStatusBG).SetActive(true);
    }

    public enum Images
    {
        Image_RangerBG, Image_ObtainedStatusBG
    }

    public enum Texts
    {
        Text_Ranger, Text_ObtainedStatus
    }

}
