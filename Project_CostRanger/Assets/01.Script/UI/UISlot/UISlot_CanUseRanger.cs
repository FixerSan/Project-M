using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlot_CanUseRanger : UISlot_PrepareRanger
{
    public RangerControllerData data;
    private Transform contentTrans;
    private Transform canvasTrans;
    
    private Vector3 worldPosition;
    private UIPrepareRanger ranger;


    public void Init(RangerControllerData _data, Transform _canvasTrans)
    {
        data = _data;
        canvasTrans = _canvasTrans;
        contentTrans = transform.parent;
        slotIndex = -1; //��밡���� �������� �����ִ� ������ �� ��ü�� �ε����� -1�� ����
        BindObject(typeof(Objects));
        BindImage(typeof(Images));
        GetObject((int)Objects.RangerTrans).gameObject.GetOrAddComponent<UIPrepareRanger>().Init(_data, _canvasTrans, this);
    }
    
    public void UseBattleEntity()
    {

    }

    public void UpdateState()
    {

    }

    public override void OnChanging()
    {
        if (GetImage((int)Images.Image_Illust).gameObject.activeSelf)
            GetImage((int)Images.Image_Illust).gameObject.SetActive(false);
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
        RangerTrans
    } 
}
