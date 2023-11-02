using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlot_CanUseBattleEntity : UIBase
{
    public BattleEntityData data;
    public SlotState slotState;
    
    public void Init(BattleEntityData _data, SlotState _state)
    {
        data = _data;
        slotState = _state;
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindObject(typeof(Objects));

        GetObject((int)Objects.Bundle_Used).gameObject.SetActive(false);
        Managers.Resource.Load<Sprite>(data.name, (_sprite) => { GetImage((int)Images.Image_Illust).sprite = _sprite; });
        GetText((int)Texts.Text_Level).text = $"Lv.{data.level}";
        BindEvent(GetButton((int)Buttons.Slot_CanUseBattleEntity).gameObject, UseBattleEntity) ;
    }
    
    public void UseBattleEntity()
    {
        if(slotState == SlotState.UnUsed)
            Managers.Game.battleInfo.UseBattleEntity(data);
    }

    public void UpdateState(SlotState _state)
    {
        slotState = _state;
        if(_state == SlotState.UnUsed)
        {
            Get<GameObject>((int)Objects.Bundle_Used).gameObject.SetActive(false);
            GetButton((int)Buttons.Slot_CanUseBattleEntity).interactable = true;
        }
        else
        {
            Get<GameObject>((int)Objects.Bundle_Used).gameObject.SetActive(true);
            GetButton((int)Buttons.Slot_CanUseBattleEntity).interactable = false;
        }
    }

    private enum Buttons
    {
        Slot_CanUseBattleEntity
    }

    private enum Images
    {
        Image_Illust
    }
    
    public enum Texts
    {
        Text_Level
    }

    public enum Objects
    {
        Bundle_Used,
    }

    public enum SlotState
    {
        UnUsed,Used
    }
}
