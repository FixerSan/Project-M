using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlot_MVPPoint : UIBase
{
    public int order;
    public void Init(int _order)
    {
        order = _order;
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetText((int)Texts.Text_MVPPoint).text = "0";
        switch(order)
        {
            case 0:
                GetImage((int)Images.Image_Point).color = Color.red;
                break;
            case 1:
                GetImage((int)Images.Image_Point).color = new Color(1,0.5f,0);
                break;
            case 2:
                GetImage((int)Images.Image_Point).color = Color.yellow;
                break;
            case 3:
                GetImage((int)Images.Image_Point).color = new Color(0.5f, 1, 0.5f);
                break;
            case 4:
                GetImage((int)Images.Image_Point).color = Color.green;
                break;
            case 5:
                GetImage((int)Images.Image_Point).color = new Color(0.5f, 1, 1);
                break;
            case 6:
                GetImage((int)Images.Image_Point).color = Color.blue;
                break;
            case 7:
                GetImage((int)Images.Image_Point).color = new Color(0, 0, 0.5f);
                break;
            case 8:
                GetImage((int)Images.Image_Point).color = new Color(0.5f, 0, 0.5f);
                break;
        }
        GetImage((int)Images.Image_Point).fillAmount = 0.1f;
    }

    public void UpdateValue()
    {
        GetText((int)Texts.Text_MVPPoint).text = $"{Managers.Game.battleInfo.battleMVPPoints[order].mvpPoint}";
        GetImage((int)Images.Image_Point).fillAmount = (float)Managers.Game.battleInfo.battleMVPPoints[order].mvpPoint / (float)Managers.Game.battleInfo.allBattlePoint;
    }


    private enum Images
    {
        Image_Point, Image_Illust
    }

    private enum Texts
    {
        Text_MVPPoint
    }
}
