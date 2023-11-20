using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;
using static UISlot_CanUseRanger;

public class UISlot_UseRanger : UISlot_PrepareRanger
{
    private UIPrepareRanger ranger;

    public override bool Init()
    {
        if (!base.Init()) return false;
        BindEvent(gameObject, OnClick_CancelUse);
        BindEvent(gameObject, _dropCallback: OnDrop, _type: UIEventType.Drop);
        return true;
    }

    public void OnClick_CancelUse()
    {
        if (Managers.Game.prepareStageSystem.rangers[slotIndex] == null) return;

        Managers.Game.prepareStageSystem.CancelUseRanger(slotIndex);
    }

    public void RedrawUI(RangerControllerData _data, Transform _canvasTrans)
    {
        if (ranger != null)
            Managers.Resource.Destroy(ranger.gameObject);

        if (_data == null) return;
        GameObject go = Managers.Resource.Instantiate("UIPrepareRanger");
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero; 
        go.transform.localScale = Vector3.one;
        ranger = go.GetOrAddComponent<UIPrepareRanger>();
        ranger.Init(_data, _canvasTrans, this);
    }


    public void OnDrop(PointerEventData _eventData)
    {
        ranger = _eventData.pointerDrag.GetComponent<UIPrepareRanger>();
        if (ranger == null) return;

        transform.SetAsFirstSibling();
        Managers.Game.prepareStageSystem.CancelUseRanger(ranger.slot.slotIndex);
        Managers.Game.prepareStageSystem.SetUseRanger(ranger.data.UID, slotIndex);
    }

    public override void OnChanging()
    {

    }
}
