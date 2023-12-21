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

        RangerInfoData data = Managers.Data.GetRangerInfoData(_uid);

        Managers.Resource.Load<Sprite>($"{data.name}", _callback: (_sprite) => { GetImage((int)Images.Image_Ranger).sprite = _sprite; });
        Managers.Resource.Load<Sprite>($"Card_{data.rarity}", (_sprite) => { GetImage((int)Images.Image_Card).sprite = _sprite; });
        Managers.Resource.Load<Sprite>($"CostPlace_{data.rarity}", (_sprite) => { GetImage((int)Images.Image_CostPlace).sprite = _sprite; });
        GetText((int)Texts.Text_Cost).text = data.cost.ToString();
        GetText((int)Texts.Text_Name).text = data.name.ToString();



        if (!_isAlreadyObtained)
        {
            GetImage((int)Images.Image_ObtainedStatusBG).gameObject.SetActive(true);
            //GetImage((int)Images.Image_NewBG).gameObject.SetActive(true);
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
        Image_Ranger, Image_RangerBG, Image_ObtainedStatusBG, Image_NewBG, Image_CostPlace, Image_Card
    }

    private enum Texts
    {
        Text_RangerName, Text_ObtainedStatus, Text_Name, Text_Cost
    }
}
