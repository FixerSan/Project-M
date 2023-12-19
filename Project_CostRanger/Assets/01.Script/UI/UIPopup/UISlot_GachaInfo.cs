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

        GetImage((int)Images.Image_RangerBG).sprite = Managers.Resource.Load<Sprite>($"{_uid}.sprite");

        GetText((int)Texts.Text_RangerName).gameObject.SetActive(true);
        GetText((int)Texts.Text_RangerName).text = $"{_uid}";

        if (!_isAlreadyObtained)
        {
            GetImage((int)Images.Image_ObtainedStatusBG).gameObject.SetActive(true);
        }

        gameObject.transform.DOScale(1f, 0.8f).SetEase(Ease.InBounce);
    }

    public void OnDisable()
    {
        GetImage((int)Images.Image_ObtainedStatusBG).gameObject.SetActive(false);
        GetImage((int)Images.Image_RangerBG).sprite = null;
    }

    public enum Images
    {
        Image_RangerBG, Image_ObtainedStatusBG
    }

    public enum Texts
    {
        Text_RangerName, Text_ObtainedStatus
    }

}
