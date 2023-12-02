using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopup_WorldMap_ChapterOne : UIPopup
{
    private Scrollbar scrollbar;
    private float scrollX;
    private Vector3 vector3 = Vector3.zero;
    public float minScrollX;
    public float maxScrollX;
    public float dragForce;

    public override bool Init()
    {
        if(!base.Init()) return false;
        scrollbar = Util.FindChild<Scrollbar>(gameObject);

        BindButton(typeof(Buttons));
        BindObject(typeof(Objects));

        BindEvent(GetObject((int)Objects.Image_Background), _dragCallback: OnDrag, _type: Define.UIEventType.Drag);

        BindEvent(GetButton((int)Buttons.Button_Close).gameObject, () => { Managers.UI.ClosePopupUI(this); });
        BindEvent(GetButton((int)Buttons.Button_StageOne).gameObject, () => 
        { 
            Managers.Game.StartPrepare(0);
        });


        return true;
    }

    public void OnChangeScrollbarValue()
    {
        if (scrollbar == null) return;
        scrollX = 1 - scrollbar.value;
        vector3.x = (1 - scrollX) * minScrollX + scrollX * maxScrollX;
        vector3 = Camera.main.ScreenToWorldPoint(vector3);
        vector3.y = 0;
        vector3.z = 0;
        GetObject((int)Objects.Image_Background).transform.position = vector3;
    }

    public void OnDrag(PointerEventData _data)
    {
        scrollbar.value += (-1) * _data.delta.x * dragForce;
        scrollbar.value = Mathf.Clamp(scrollbar.value, 0, 1);
    }

    public override void RedrawUI()
    {

    }

    private enum Buttons
    {
        Button_StageOne, Button_StageTwo, Button_Close
    }

    private enum Objects
    {
        Image_Background
    }
}
