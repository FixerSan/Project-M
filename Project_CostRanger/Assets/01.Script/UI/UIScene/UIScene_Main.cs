using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIScene_Main : UIScene
{
    private bool isRangerToggled = false;

    public override bool Init()
    {
        if (!base.Init())
            return false;

        BindObject(typeof(Transforms));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        BindEvent(GetButton((int)Buttons.Button_Battle).gameObject, () => { Managers.UI.ShowPopupUI<UIPopup_WorldMap_ChapterOne>(); });
        //BindEvent(GetButton((int)Buttons.Button_Gacha).gameObject, () => { Managers.UI.ShowPopupUI<UIPopup_Gacha>(); });

        BindEvent(GetButton((int)Buttons.Button_Ranger).gameObject, _callback: OnClick_Ranger);
        BindEvent(GetButton((int)Buttons.Button_RangerList).gameObject, OnClick_List);
        BindEvent(GetButton((int)Buttons.Button_RangerDraw).gameObject, () => { Managers.UI.ShowPopupUI<UIPopup_Gacha>(); });
        BindEvent(GetButton((int)Buttons.Button_RangerStatUP).gameObject, () => { });

        isRangerToggled = false;

        RedrawUI();
        return true;
    }

    public override void RedrawUI()
    {
        GetText((int)Texts.Text_UserName).text = $"{Managers.Game.playerData.name}";
        GetText((int)Texts.Text_UserLevel).text = $"LV.{Managers.Game.playerData.level}";
        GetText((int)Texts.Text_UserGem).text = $"{Managers.Game.playerData.gem}";
        GetText((int)Texts.Text_UserGold).text = $"{Managers.Game.playerData.gold}";
        // GetText((int)Texts.Text_EXP).text = $"{Managers.Game.playerData.exp}";

        // GetImage((int)Images.Image_EXPGauge).fillAmount = n(currentEXP) / N(maxEXP);
        // Managers.Resource.Load<Sprite>(레인저 이름, (_sprite) => { GetImage((int)Images.Image_Illust).sprite = _sprite}

    }

    public void OnClick_Ranger()
    {
        Sequence sequence = DOTween.Sequence();

        if (!isRangerToggled)
            sequence.OnStart(() =>
            {
                isRangerToggled = true;
                GetObject((int)Transforms.Transform_RangerButtons).gameObject.transform.
                DOMoveX(GetObject((int)Transforms.Transform_RangerButtonsActive).transform.position.x, 0.4f)
                .SetEase(Ease.OutQuart);
            });
        else
            sequence.OnStart(() =>
            {
                isRangerToggled = false;
                GetObject((int)Transforms.Transform_RangerButtons).gameObject.transform.
                DOMoveX(GetObject((int)Transforms.Transform_RangerButtonsNotActive).transform.position.x, 0.4f)
                .SetEase(Ease.OutQuart);
            });
    }

    public void OnClick_List()
    {
        Managers.UI.ShowPopupUI<UIPopup_RangerList>();
    }

    private enum Transforms
    {
        Transform_RangerButtons, Transform_RangerButtonsActive, Transform_RangerButtonsNotActive
    }

    private enum Images
    {
        Image_EXPGauge, Image_Illust
    }

    private enum Buttons
    {
        Button_Battle, Button_Draw, Button_Ranger, Button_RangerList, Button_RangerDraw, Button_RangerStatUP,
    }

    private enum Texts
    {
        Text_UserName, Text_UserLevel, Text_UserGem, Text_UserGold, Text_EXP
    }
}
