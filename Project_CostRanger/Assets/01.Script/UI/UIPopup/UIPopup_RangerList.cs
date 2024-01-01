using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UISlot_CanUseRanger;

public class UIPopup_RangerList : UIPopup
{
    private List<UISlot_RangerList> slots = new List<UISlot_RangerList>();
    public override bool Init()
    {
        if (!base.Init()) return false;
        BindImage(typeof(Images));
        BindObject(typeof(Objects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        BindEvent(GetButton((int)Buttons.Button_Close).gameObject, ClosePopupUP);

        GetObject((int)Objects.Bundle_RangerInfo).SetActive(false);

        DrawSlot();
        return true;
    }

    public void DrawSlot()
    {
        for (int i = 0; i < Managers.Game.playerData.hasRangers.Count; i++)
        {
            UISlot_RangerList slot = Managers.Resource.Instantiate("UISlot_RangerList").GetComponent<UISlot_RangerList>();
            slot.transform.parent = GetObject((int)Objects.Content_RangeListSlot).transform;
            slot.transform.localScale = Vector3.one;
            slot.Init(Managers.Data.GetRangerControllerData(Managers.Game.playerData.hasRangers[i].UID));
            slots.Add(slot);
        }
    }

    public void OpenRangerInfo(RangerControllerData _data)
    {
        GetObject((int)Objects.Bundle_RangerInfo).SetActive(true);

        Managers.Resource.Load<Sprite>($"Card_{_data.rarity}", (_sprite) => { GetImage((int)Images.Image_Card).sprite = _sprite; });
        Managers.Resource.Load<Sprite>($"CostPlace_{_data.rarity}", (_sprite) => { GetImage((int)Images.Image_CostPlace).sprite = _sprite; });
        Managers.Resource.Load<Sprite>(_data.name, (_sprite) => { GetImage((int)Images.Image_Illust).sprite = _sprite; });
        GetText((int)Texts.Text_Cost).text = _data.cost.ToString();
        if (_data.level != 0)
            GetText((int)Texts.Text_Level).text = $"+{_data.level}";
        GetText((int)Texts.Text_Name).text = _data.name.ToString();
        GetText((int)Texts.Text_HP).text = _data.hp.ToString();
        GetText((int)Texts.Text_AttackForce).text = _data.attackForce.ToString();
        GetText((int)Texts.Text_Defense).text = _data.defenseForce.ToString();
        GetText((int)Texts.Text_AttackTypeOne).text = _data.attackType.ToString();
        GetText((int)Texts.Text_AttackTypeTwo).text = string.Empty;
        GetText((int)Texts.Text_MoveSpeed).text = _data.moveSpeed.ToString();
        GetText((int)Texts.Text_SpecialtyOne).text = _data.specialtyOne.ToString();
        GetText((int)Texts.Text_SpecialtyTwo).text = string.Empty;
        if(_data.specialtyTwo != null &&_data.specialtyTwo != string.Empty)
            GetText((int)Texts.Text_SpecialtyTwo).text = _data.specialtyTwo.ToString(); 
    }

    private enum Objects
    {
        Bundle_RangerInfo, Content_RangeListSlot
    }

    private enum Images
    {
        Image_Card,
        Image_Illust,
        Image_CostPlace,
    }

    private enum Texts
    {
        Text_Cost,
        Text_Level,
        Text_Name,
        Text_HP,
        Text_AttackForce,
        Text_Defense,
        Text_AttackTypeOne,
        Text_AttackTypeTwo,
        Text_MoveSpeed,
        Text_SpecialtyOne,
        Text_SpecialtyTwo
    }

    private enum Buttons
    {
        Button_Close
    }
}
