using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UISlot_UseBattleEntity : UIBase
{
    public BattleEntityData data;
    private CanvasGroup canvasGroup;
    private Transform slotDragTransform;
    private Transform firstTrans;
    private SlotType slotType;
    private bool isDraging = false;

    public void Init(BattleEntityData _data, SlotType _type)
    {
        data = _data;
        slotType = _type;
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        canvasGroup = GetComponent<CanvasGroup>();
        slotDragTransform = Util.FindChild(Managers.UI.Root.gameObject, "Bundle_ArmyBattleEntitySpaces", true).transform;


        Managers.Resource.Load<Sprite>(data.name, (_sprite) => { GetImage((int)Images.Slot_UseBattleEntity).sprite = _sprite; });

        GetText((int)Texts.Text_NameAndLevel).text = $"{_data.name} Lv.{_data.level}";
        if (_type == SlotType.Enemy)
        {
            GetButton((int)Buttons.Slot_UseBattleEntity).interactable = false;
            transform.eulerAngles = new Vector3(0, 180, 0);
            GetText((int)Texts.Text_NameAndLevel).transform.eulerAngles = new Vector3(0, 0, 0);
            return;
        }

        BindEvent(GetButton((int)Buttons.Slot_UseBattleEntity).gameObject, RemoveSlot);
        BindEvent(GetButton((int)Buttons.Slot_UseBattleEntity).gameObject, _dracCallback: EndDrag, _type:Define.UIEventType.EndDrag);
        BindEvent(GetButton((int)Buttons.Slot_UseBattleEntity).gameObject, _dracCallback: (_) => { if (slotType == SlotType.Enemy) return; DragMove(_); }, _type:Define.UIEventType.Drag);

        firstTrans = transform;
    }

    public void DragMove(PointerEventData _data)
    {
        isDraging = true;
        Transform dragGameObjectTrans = _data.pointerDrag.transform;
        transform.SetParent(slotDragTransform);
        transform.position = new Vector3(dragGameObjectTrans.position.x + _data.delta.x, dragGameObjectTrans.position.y + _data.delta.y, dragGameObjectTrans.position.z);
        canvasGroup.blocksRaycasts = false;
    }

    public void EndDrag(PointerEventData _data)
    {
        Managers.Game.battleInfo.UpdateUI();

        isDraging = false;
        canvasGroup.blocksRaycasts = true;
    }

    public void RemoveSlot()
    {
        if (isDraging) 
            return;
        
        Managers.Game.battleInfo.UnUseBattleEntity(data);
    }

    private enum Buttons
    {
        Slot_UseBattleEntity
    }

    private enum Images
    {
        Slot_UseBattleEntity
    }

    private enum Texts
    {
        Text_NameAndLevel
    }


    public enum SlotType
    {
        Army, Enemy
    }
}
