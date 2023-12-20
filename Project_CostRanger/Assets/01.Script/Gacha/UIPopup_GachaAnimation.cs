using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

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

        var obtainedPool = Managers.Gacha.GetGachaResult();
        bool isEpic = false;
        bool isLegendary = false;

        foreach (var item in obtainedPool)
        {
            // 각 가챠 유닛의 확률을 전부 구해서
            // 만약 하나라도 레전드 이상일 경우 -> 레어 가챠 연출

            var data = Managers.Data.GetRangerInfoData(item);

            if (data != null && data.UID != 0)
            {
                if (data.rarity == "Epic" && isEpic == false)
                {
                    Debug.Log("이쉬끼 Epic인데요?");
                    isEpic = true;
                }

                if (data.rarity == "Legendary" && isLegendary == false)
                {
                    Debug.Log("이쉬끼 Legendary인데요?");
                    isEpic = true;
                    isLegendary = true;
                }
            }
        }

        if (isEpic)
        {
            raritySequence
            .Append(GetImage((int)Images.Image_RecruitmentPaper).transform.DOScale(1.2f, 0.5f).SetEase(Ease.InQuart))
            .Join(GetImage((int)Images.Image_GlowEffect).DOColor(Color.white, 0.5f).SetEase(Ease.InQuart))
            .Append(GetImage((int)Images.Image_RecruitmentPaper).transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuart))
            .Join(GetImage((int)Images.Image_RecruitmentPaper).DOColor(new Color(148f / 255f, 0, 211f / 255f), 0.5f).SetEase(Ease.OutQuart))
            .Join(GetImage((int)Images.Image_GlowEffect).DOColor(new Color(148f / 255f, 0, 211f / 255f, 0f/255f), 0.5f).SetEase(Ease.OutQuart))
            .Join(GetImage((int)Images.Image_GlowEffect).transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutQuart))
            .Join(GetImage((int)Images.Image_HaloEffect).DOColor(new Color(148f / 255f, 0, 211f / 255f), 0.1f))
            .Append(GetImage((int)Images.Image_GlowEffect).transform.DOScale(1f, 0f).SetEase(Ease.OutQuart))
            .Join(GetImage((int)Images.Image_GlowEffect).DOColor(Color.clear, 0f).SetEase(Ease.InQuart))
            ;
        }

        if (isLegendary)
        {
            raritySequence
            .Append(GetImage((int)Images.Image_RecruitmentPaper).transform.DOScale(1.2f, 0.5f).SetEase(Ease.InQuart))
            .Join(GetImage((int)Images.Image_GlowEffect).DOColor(Color.white, 0.5f).SetEase(Ease.InQuart))
            .Append(GetImage((int)Images.Image_RecruitmentPaper).transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuart))
            .Join(GetImage((int)Images.Image_RecruitmentPaper).DOColor(Color.yellow, 0.5f).SetEase(Ease.OutQuart))
            .Join(GetImage((int)Images.Image_GlowEffect).DOColor(new Color(255f/255f, 220f/255f, 8f/255f, 0f/255f), 0.5f).SetEase(Ease.OutQuart))
            .Join(GetImage((int)Images.Image_GlowEffect).transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutQuart))
            .Join(GetImage((int)Images.Image_HaloEffect).DOColor(Color.yellow, 0.1f))
            .Append(GetImage((int)Images.Image_GlowEffect).transform.DOScale(1f, 0f).SetEase(Ease.OutQuart))
            .Join(GetImage((int)Images.Image_GlowEffect).DOColor(Color.clear, 0f).SetEase(Ease.InQuart))
            ;
        }

        TweenerCore<Quaternion, Vector3, QuaternionOptions> loopTweener = default;
        var mySequence = DOTween.Sequence().OnStart(() =>
    {
        loopTweener = GetImage((int)Images.Image_HaloEffect).transform.DORotate(new Vector3(0, 0, 180), 0.2f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);

    })
    .Append(GetImage((int)Images.Image_RecruitmentPaper).transform.DOMoveY(0, 2f).SetEase(Ease.OutQuart))
    //.Append(GetImage((int)Images.Image_HaloEffect).transform.DOMoveX(0, 3f).SetEase(Ease.Linear))
    .Append(raritySequence)
    .Append(GetImage((int)Images.Image_HaloEffect).transform.DOScale(6f, 3)).SetEase(Ease.Linear)
    .Append(GetImage((int)Images.Image_Block).DOColor(Color.white, 1f))
    .OnComplete(() =>
    {
        loopTweener.Kill();
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UIPopup_GachaResult>();
    });
    }

    private enum Buttons
    {

    }

    private enum Images
    {
        Image_BG, Image_HaloEffect, Image_RecruitmentPaper, Image_Block, Image_GlowEffect
    }
}
