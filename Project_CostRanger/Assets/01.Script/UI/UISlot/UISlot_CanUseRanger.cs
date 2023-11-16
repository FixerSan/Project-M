using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlot_CanUseRanger : UIBase
{
    public RangerInfoData data;
    private Transform contentTrans;
    private Transform canvas;

    private Vector3 worldPosition;


    public void Init(RangerInfoData _data, Transform _canvas)
    {
        data = _data;
        canvas = _canvas;
        contentTrans = transform.parent;
        BindEvent(gameObject, _dragCallback: OnDrag, _type: Define.UIEventType.Drag);
        BindEvent(gameObject, _dragCallback: OnEndDrag, _type: Define.UIEventType.EndDrag);
    }

    public void OnDrag(PointerEventData _eventData)
    {
        transform.SetParent(canvas);
        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = canvas.position.z;
        gameObject.transform.position = worldPosition;
    }

    public void OnEndDrag(PointerEventData _eventData)
    {
        transform.SetParent(contentTrans);
    }
    
    public void UseBattleEntity()
    {

    }

    public void UpdateState()
    {

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
}
