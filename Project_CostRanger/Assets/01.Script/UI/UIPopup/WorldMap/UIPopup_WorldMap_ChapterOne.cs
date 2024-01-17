using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopup_WorldMap_ChapterOne : UIPopup
{
    private Scrollbar scrollbar;
    private float scrollX;
    private Vector3 vector3 = Vector3.zero;
    public float minScrollX;
    public float maxScrollX;
    public float dragForce;

    public StageProfile chapterOneStageProfle;
    public override bool Init()
    {
        if (!base.Init()) return false;
        scrollbar = Util.FindChild<Scrollbar>(gameObject);

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindObject(typeof(Objects));

        // BindEvent(GetObject((int)Objects.Image_Background), _dragCallback: OnDrag, _type: Define.UIEventType.Drag);

        BindEvent(GetButton((int)Buttons.Button_Back).gameObject, () => { Managers.UI.ClosePopupUI(this); });
        BindEvent(GetButton((int)Buttons.Button_StageOne).gameObject, () => { Managers.UI.ShowPopupUI<UIPopup_StageInfo>().Init(chapterOneStageProfle.stageOneUID); });
        BindEvent(GetButton((int)Buttons.Button_StageTwo).gameObject, () => { Managers.UI.ShowPopupUI<UIPopup_StageInfo>().Init(chapterOneStageProfle.stageTwoUID); });
        BindEvent(GetButton((int)Buttons.Button_StageThree).gameObject, () => { Managers.UI.ShowPopupUI<UIPopup_StageInfo>().Init(chapterOneStageProfle.stageThreeUID); });
        BindEvent(GetButton((int)Buttons.Button_StageFour).gameObject, () => { Managers.UI.ShowPopupUI<UIPopup_StageInfo>().Init(chapterOneStageProfle.stageFourUID); });
        BindEvent(GetButton((int)Buttons.Button_StageFive).gameObject, () => { Managers.UI.ShowPopupUI<UIPopup_StageInfo>().Init(chapterOneStageProfle.stageFiveUID); });

        switch(Managers.Game.playerData.lastClearStageUID)
        {
            case 0:

                break;
            case 101:
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageOneStar).sprite = _sprite; });
                break;

            case 102:
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageOneStar).sprite = _sprite; });
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageTwoStar).sprite = _sprite; });

                break;

            case 103:
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageOneStar).sprite = _sprite; });
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageTwoStar).sprite = _sprite; });
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageThreeStar).sprite = _sprite; });

                break;

            case 104:
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageOneStar).sprite = _sprite; });
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageTwoStar).sprite = _sprite; });
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageThreeStar).sprite = _sprite; });
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageFourStar).sprite = _sprite; });

                break;

            case 105:
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageOneStar).sprite = _sprite; });
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageTwoStar).sprite = _sprite; });
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageThreeStar).sprite = _sprite; });
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageFourStar).sprite = _sprite; });
                Managers.Resource.Load<Sprite>("ChapterStarsClear", (_sprite) => { GetImage((int)Images.Image_StageFiveStar).sprite = _sprite; });
                break;
        }



        return true;
    }

    //public void OnChangeScrollbarValue()
    //{
    //    if (scrollbar == null) return;
    //    scrollX = 1 - scrollbar.value;
    //    vector3.x = (1 - scrollX) * minScrollX + scrollX * maxScrollX;
    //    vector3 = Camera.main.ScreenToWorldPoint(vector3);
    //    vector3.y = 0;
    //    vector3.z = 0;
    //    GetObject((int)Objects.Image_Background).transform.position = vector3;
    //}

    //public void OnDrag(PointerEventData _data)
    //{
    //    scrollbar.value += (-1) * _data.delta.x * dragForce;
    //    scrollbar.value = Mathf.Clamp(scrollbar.value, 0, 1);
    //}

    public override void RedrawUI()
    {
        //if( 1 스테이지 클리어할 경우)
        //{
        //    Managers.Resource.Load<Sprite>("별 n개 클리어.sprite", (_sprite) => { GetImage((int)Images.Image_StageOneStar).sprite = _sprite; });
        //    Managers.Resource.Load<Sprite>("스테이지 클리어 선.sprite", (_sprite) => { GetImage((int)Images.Image_StageOneClear).sprite = _sprite; });
        //}

        // 복붙해서 여러개 만들기
    }


    private enum Texts
    {
        Text_StageOne, Text_StageTwo, Text_StageThree, Text_StageFour, Text_StageFive,
        Text_UserGem, Text_UserGold, Text_UserEXP
    }

    private enum Buttons
    {
        Button_StageOne, Button_StageTwo, Button_StageThree, Button_StageFour, Button_StageFive, Button_Back
    }

    private enum Objects
    {
        Image_Background
    }

    private enum Images
    {
        Image_StageOneClear, Image_StageTwoClear, Image_StageThreeClear, Image_StageFourClear,
        Image_StageOneStar, Image_StageTwoStar, Image_StageThreeStar, Image_StageFourStar, Image_StageFiveStar
    }
}
