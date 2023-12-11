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
        slotIndex = -1; //사용가능한 레인저를 보여주는 슬롯인 이 객체의 인덱스는 -1로 설정
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindObject(typeof(Objects));

        Managers.Resource.Load<Sprite>($"Card_{_data.rarity}", (_sprite) => { GetImage((int)Images.Image_Card).sprite = _sprite; });
        Managers.Resource.Load<Sprite>($"CostPlace_{_data.rarity}", (_sprite) => { GetImage((int)Images.Image_CostPlace).sprite = _sprite; });
        Managers.Resource.Load<Sprite>(_data.name, (_sprite) => { GetImage((int)Images.Image_Illust).sprite = _sprite; });

        GetText((int)Texts.Text_Cost).text = data.cost.ToString();
        if(data.level != 0)
            GetText((int)Texts.Text_Level).text = $"+{data.level}";
        GetText((int)Texts.Text_Name).text = data.name.ToString();

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
        if (GetObject((int)Objects.Bundle_GameObject).activeSelf)
            GetObject((int)Objects.Bundle_GameObject).SetActive(false);
    }

    private enum Images
    {
        Image_Illust, Image_Card, Image_CostPlace
    }
    
    public enum Texts
    {
        Text_Level, Text_Cost, Text_Name
    }

    public enum Objects
    {
        RangerTrans, Bundle_GameObject
    } 
}
