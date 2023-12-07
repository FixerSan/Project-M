using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlot_StageRangerSkill : UIBase
{
    private RangerController controller;
    public void Init(RangerController _controller)
    {
        controller = _controller;

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindObject(typeof(Objects));

        GetObject((int)Objects.Bundle_Die).SetActive(false);

        Managers.Resource.Load<Sprite>(controller.data.name, (_sprite) => { GetImage((int)Images.Image_Illust).sprite = _sprite; });
        BindEvent(gameObject, OnClick);
    }

    public void OnClick()
    {
        controller.ranger.UseSkill();
    }

    public void Update()
    {
        if (controller == null) return;

        if(controller.currentState == Define.RangerState.Die)
        {
            if (GetObject((int)Objects.Bundle_Die).gameObject.activeSelf) return;
            GetObject((int)Objects.Bundle_Die).SetActive(true);
            GetText((int)Texts.Text_Cooltime).gameObject.SetActive(false);
            return;
        }

        if (controller.status.CheckSkillCooltime == 0)
        {
            if(GetImage((int)Images.Image_Cooltime).gameObject.activeSelf)
                GetImage((int)Images.Image_Cooltime).gameObject.SetActive(false);

            if (GetText((int)Texts.Text_Cooltime).gameObject.activeSelf)
                GetText((int)Texts.Text_Cooltime).gameObject.SetActive(false);
            return;
        }

        if (!GetImage((int)Images.Image_Cooltime).gameObject.activeSelf)
            GetImage((int)Images.Image_Cooltime).gameObject.SetActive(true);

        if (!GetText((int)Texts.Text_Cooltime).gameObject.activeSelf)
            GetText((int)Texts.Text_Cooltime).gameObject.SetActive(true);

        GetText((int)Texts.Text_Cooltime).text = Mathf.Floor(controller.status.CheckSkillCooltime).ToString();
    }

    private enum Texts
    {
        Text_Cooltime
    }

    private enum Images
    {
        Image_Illust , Image_Cooltime
    }

    private enum Objects
    {
        Bundle_Die,
    }
}
