using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using DG.Tweening;

public class UISlot_GachaInfo : UIBase
{
    public override bool Init()
    {
        if (base.Init() == true)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        return true;
    }

    public void Redraw(int _uid, bool _isAlreadyObtained)
    {
        Init();

        gameObject.SetActive(true);
        gameObject.transform.localScale = Vector3.zero;

        GetImage((int)Images.Image_ObtainedStatusBG).gameObject.SetActive(false);
        GetImage((int)Images.Image_NewBG).gameObject.SetActive(false);

        GetImage((int)Images.Image_Ranger).sprite = Managers.Resource.Load<Sprite>($"{_uid}.sprite");

        if (!_isAlreadyObtained)
        {
            GetImage((int)Images.Image_ObtainedStatusBG).gameObject.SetActive(true);
            GetImage((int)Images.Image_NewBG).gameObject.SetActive(true);
        }

        gameObject.transform.DOScale(1f, 0.8f).SetEase(Ease.InBounce);
    }

    public void OnDisable()
    {
        GetImage((int)Images.Image_ObtainedStatusBG).gameObject.SetActive(false);
        GetImage((int)Images.Image_NewBG).gameObject.SetActive(false);
        GetImage((int)Images.Image_Ranger).sprite = null;
    }

    private enum Images
    {
        Image_Ranger, Image_RangerBG, Image_ObtainedStatusBG, Image_NewBG
    }

    private enum Texts
    {
        Text_RangerName, Text_ObtainedStatus
    }
}
