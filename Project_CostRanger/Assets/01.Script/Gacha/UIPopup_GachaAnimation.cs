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

        GetImage((int)Images.Image_HaloEffect).transform.DOMoveX(0, 3f).SetEase(Ease.InOutBounce).OnComplete(() =>
        {
            GetImage((int)Images.Image_HaloEffect).transform.DOScale(6f, 1).SetEase(Ease.InQuart);
        });

        return true;
    }

    public void OnClick_ShowChart()
    {
        Managers.UI.ShowPopupUI<UIPopup_GachaChart>();
    }

    private void OnEnable()
    {
        var mySequence = DOTween.Sequence().OnStart(() =>
    {
        GetImage((int)Images.Image_HaloEffect).transform.DORotate(new Vector3(0, 0, 180), 0.2f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    })
    .Append(GetImage((int)Images.Image_HaloEffect).transform.DOMoveX(0, 3f).SetEase(Ease.OutSine))
    .Append(GetImage((int)Images.Image_HaloEffect).transform.DOScale(6f, 1).SetEase(Ease.OutSine))
    .Join(GetImage((int)Images.Image_BG).DOColor(Color.white, 1f)).SetEase(Ease.InSine)
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
        Image_BG, Image_HaloEffect
    }
}
