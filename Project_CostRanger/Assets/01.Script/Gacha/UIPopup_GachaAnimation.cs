using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIPopup_GachaAnimation : UIPopup
{
    public override bool Init()
    {
        if (!base.Init())
            return false;

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        return true;
    }

    public void OnClick_ShowChart()
    {
        Managers.UI.ShowPopupUI<UIPopup_GachaChart>();
    }

    private void OnEnable()
    {
        GetImage((int)Images.Image_HaloEffect).transform.localScale = Vector3.zero;

        Sequence raritySequence = DOTween.Sequence();

        foreach (var item in Managers.Gacha.GetGachaResult())
        {
            // 아이템 확률에 따라 연출 변경
        }

        if (Random.Range(0, 2) == 0)
        {
            Debug.Log("레어 가챠");
            raritySequence
            .Append(GetImage((int)Images.Image_RecruitmentPaper).transform.DOScale(1.4f, 0.5f).SetEase(Ease.InQuart))
            .Join(GetImage((int)Images.Image_RecruitmentPaper).DOColor(Color.white, 0.5f).SetEase(Ease.InQuart))
            .Append(GetImage((int)Images.Image_RecruitmentPaper).transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuart))
            .Join(GetImage((int)Images.Image_RecruitmentPaper).DOColor(Color.yellow, 0.5f).SetEase(Ease.OutQuart))
            .Join(GetImage((int)Images.Image_HaloEffect).DOColor(Color.yellow, 0.1f))
            ;
        }
        else
        {
            Debug.Log("일반가챠");

        }



        var mySequence = DOTween.Sequence().OnStart(() =>
    {
        GetImage((int)Images.Image_HaloEffect).transform.DORotate(new Vector3(0, 0, 180), 0.2f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    })
    .Append(GetImage((int)Images.Image_RecruitmentPaper).transform.DOMoveY(0, 2f).SetEase(Ease.OutQuart))
    //.Append(GetImage((int)Images.Image_HaloEffect).transform.DOMoveX(0, 3f).SetEase(Ease.Linear))
    .Append(raritySequence)
    .Append(GetImage((int)Images.Image_HaloEffect).transform.DOScale(6f, 3)).SetEase(Ease.Linear)
    .Append(GetImage((int)Images.Image_Block).DOColor(Color.white, 1f))
    .OnComplete(() =>
    {
        Managers.UI.ClosePopupUI(this);
        Debug.Log("애니메이션 끝");
        Managers.UI.ShowPopupUI<UIPopup_GachaResult>();
    });
    }

    private enum Buttons
    {

    }

    private enum Images
    {
        Image_BG, Image_HaloEffect, Image_RecruitmentPaper, Image_Block
    }
}
