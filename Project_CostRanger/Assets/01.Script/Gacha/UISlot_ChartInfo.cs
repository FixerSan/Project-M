using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlot_ChartInfo : UIBase
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
        var rangerInfo = Managers.Data.GetRangerInfoData(_uid);

        GetObject((int)Images.Image_Thumbnail).SetActive(true);
        GetImage((int)Images.Image_Thumbnail).sprite = Managers.Resource.Load<Sprite>($"{_uid}.sprite");

        GetText((int)Texts.Text_RangerName).text = rangerInfo.name;
        
        // rangerInfo에 레어도 추가 필요 이걸 왜 안 했지
        // GetText((int)Texts.Text_RangerRarity).text = rangerInfo.rarity;
        // GetText((int)Texts.Text_RangerProbability).text =
    }

    public void OnEnable()
    {

    }

    public void OnDisable()
    {
        // GetObject((int)Images.Image_ObtainedStatusBG).SetActive(false);
        // GetImage((int)Images.Image_RangerBG).sprite = null;
    }

    public enum Images
    {
        Image_Thumbnail
    }

    public enum Texts
    {
        Text_RangerRarity, Text_RangerName, Text_RangerProbability
    }
}
