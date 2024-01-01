using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlot_RangerList : UIBase
{
    private RangerControllerData data;
    public void Init(RangerControllerData _data)
    {
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindEvent(gameObject, OnClick);

        Redraw(_data);
    }

    public void Redraw(RangerControllerData _data)
    {
        data = _data;
        Managers.Resource.Load<Sprite>($"Card_{_data.rarity}", (_sprite) => { GetImage((int)Images.Image_Card).sprite = _sprite; });
        Managers.Resource.Load<Sprite>($"CostPlace_{_data.rarity}", (_sprite) => { GetImage((int)Images.Image_CostPlace).sprite = _sprite; });
        Managers.Resource.Load<Sprite>(_data.name, (_sprite) => { GetImage((int)Images.Image_Illust).sprite = _sprite; });

        //GetText((int)Texts.Text_Cost).text = _data.cost.ToString();
        if (_data.level != 0)
            GetText((int)Texts.Text_Level).text = $"+{_data.level}";
        GetText((int)Texts.Text_Name).text = _data.name.ToString();
    }

    public void OnClick()
    {
        if (Managers.UI.activePopups.TryGetValue(Define.UIType.UIPopup_RangerList, out UIPopup _popup))
        {
            UIPopup_RangerList popup;
            popup = _popup as UIPopup_RangerList;
            popup.OpenRangerInfo(data);
        }
    }

    private enum Images
    {
        Image_Illust, Image_Card, Image_CostPlace
    }

    public enum Texts
    {
        Text_Level, Text_Cost, Text_Name
    }

}
