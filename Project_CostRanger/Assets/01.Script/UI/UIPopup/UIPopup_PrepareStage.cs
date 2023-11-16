using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_PrepareStage : UIPopup
{
    private List<UISlot_CanUseRanger> canUseRangerSlots;
    private Transform canUseRangerSlotParent;
    public override bool Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
        BindButton(typeof(Buttons));
        canUseRangerSlots = new List<UISlot_CanUseRanger>();
        canUseRangerSlotParent = Util.FindChild(gameObject, _name: "Content_CanUseSlot", _recursive: true).transform;

        BindEvent(GetButton((int)Buttons.Button_Back).gameObject, () => { Managers.UI.ClosePopupUI(this); });
        BindEvent(GetButton((int)Buttons.Button_Start).gameObject, OnClick_Start);

        Managers.Event.OnVoidEvent -= RedrawUI;
        Managers.Event.OnVoidEvent += RedrawUI;

        RedrawCanUseSlot();
        return true;
    }

    public void OnClick_Start()
    {
        Managers.Game.StartBattleStage((callback) => 
        {
            if(callback == Define.StartBattleStageEvent.RangerIsNotExist)
            {

                return;
            }
        });
    }

    public void RedrawUI(Define.VoidEventType _type)
    {
        if (_type != Define.VoidEventType.OnChangePrepare) return;

        RedrawCanUseSlot();
    }

    public void RedrawCanUseSlot()
    {
        for (int i = canUseRangerSlots.Count - 1; i >= 0; i--)
        {
            Managers.Resource.Destroy(canUseRangerSlots[i].gameObject);
        }

        canUseRangerSlots.Clear();
        //여기서 전부 다 다시 소환하는 게 아니라 사용하고 있는 녀석이라면 스킵을 해야함
        //PrepareSystem에서 사용하고 있는 녀석인지 체크를 하면 좋을 것 같음
        for (int i = 0; i < Managers.Game.playerData.hasRangers.Count; i++)
        {
            UISlot_CanUseRanger slot = Managers.Resource.Instantiate("UISlot_CanUseRanger", canUseRangerSlotParent).GetOrAddComponent<UISlot_CanUseRanger>();
            slot.Init(Managers.Game.playerData.hasRangers[i], transform);
            canUseRangerSlots.Add(slot);
        }
    }

    private enum Buttons
    {
        Button_Start, Button_Back
    }
}
