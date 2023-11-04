using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_Result : UIPopup
{
    public void Init(Define.GameResult _result)
    {
        BindObject(typeof(Objects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        BindEvent(gameObject, () => { Managers.Scene.LoadScene(Define.Scene.Main); });

        for (int i = 0; i < GetObject((int)Objects.Bundle_Star).transform.childCount; i++)
        {
            GetObject((int)Objects.Bundle_Star).transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < Managers.Game.battleInfo.clearStar; i++)
        {
            GetObject((int)Objects.Bundle_Star).transform.GetChild(i).gameObject.SetActive(true);
        }

        Managers.Resource.Load<Sprite>(Managers.Game.battleInfo.battleMVPPoints[0].entity.data.name, (_sprite) => { GetImage((int)Images.Illust_MVP).sprite = _sprite; });

        GetText((int)Texts.Text_Result).text = _result.ToString();
        GetText((int)Texts.Text_Stage).text = $"{Managers.Game.battleInfo.currentStage.stageName}";
    }

    private enum Objects
    {
        Bundle_Star
    }

    private enum Texts
    {
        Text_Result, Text_Stage
    }

    private enum Images
    {
        Illust_MVP
    }
}
