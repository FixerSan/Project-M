using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlot_StageEnemy : UIBase
{
    private EnemyController controller;

    public void Init(EnemyController _controller)
    {
        controller = _controller;
        BindText(typeof(Texts));
        BindImage(typeof(Images));


        GetText((int)Texts.Text_Cost).text = controller.data.cost.ToString();
        GetText((int)Texts.Text_Name).text = controller.data.name;
        GetText((int)Texts.Text_Level).text = "1";

        GetImage((int)Images.Image_HPbar).fillAmount = 1;
        Managers.Resource.Load<Sprite>(controller.data.name, (_sprite) => { GetImage((int)Images.Image_Illust).sprite = _sprite; });
        Managers.Resource.Load<Sprite>(controller.data.name, (_sprite) => { GetImage((int)Images.Image_Illust).sprite = _sprite; });
        Managers.Resource.Load<Sprite>($"Card_{controller.data.cost}", (_sprite) => { GetImage((int)Images.Image_Card).sprite = _sprite; });
        Managers.Resource.Load<Sprite>($"CostPlace_{controller.data.cost}", (_sprite) => { GetImage((int)Images.Image_CostPlace).sprite = _sprite; });
    }

    public void Redraw()
    {
        GetImage((int)Images.Image_HPbar).fillAmount = controller.status.CurrentHP / controller.status.CurrentMaxHP;
    }

    private enum Texts
    {
        Text_Cost, Text_Name, Text_Level
    }

    private enum Images
    {
        Image_HPbar, Image_Illust, Image_Card, Image_CostPlace
    }
}
