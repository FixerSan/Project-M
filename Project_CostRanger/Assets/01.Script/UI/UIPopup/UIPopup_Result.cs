using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_Result : UIPopup
{
    public void Init(Define.GameResult _result, int _resultUID)
    {
        BindObject(typeof(Objects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        BindEvent(GetButton((int)Buttons.Button_FullScreen).gameObject, OnClick_FullScreen);
        BindEvent(GetButton((int)Buttons.Button_Exit).gameObject, OnClick_Exit);
        BindEvent(GetButton((int)Buttons.Button_Retry).gameObject, OnClick_Retry);
        BindEvent(GetButton((int)Buttons.Button_Draw).gameObject, OnClick_Draw);
        BindEvent(GetButton((int)Buttons.Button_Enhance).gameObject, OnClick_Enhance);

        GetObject((int)Objects.Victory).SetActive(false);
        GetObject((int)Objects.Lose).SetActive(false);

        if (_result == Define.GameResult.Victory) Victory();
        if (_result == Define.GameResult.Lose) Lose();
    }

    public void Victory()
    {
        GetObject((int)Objects.Victory).SetActive(true);
        GetObject((int)Objects.Lose).SetActive(false);
    }

    public void Lose()
    {
        GetObject((int)Objects.Victory).SetActive(false);
        GetObject((int)Objects.Lose).SetActive(true);
    }

    public void OnClick_FullScreen()
    {
        Managers.Scene.LoadScene(Define.Scene.Main);
    }

    public void OnClick_Exit()
    {
        Managers.Scene.LoadScene(Define.Scene.Stage);
    }

    public void OnClick_Retry()
    {
        Managers.Scene.LoadScene(Define.Scene.Main);
    }

    public void OnClick_Draw()
    {
        Managers.Scene.LoadScene(Define.Scene.Main);
    }

    public void OnClick_Enhance()
    {
        // °­È­
    }

    public override void RedrawUI()
    {

    }

    private enum Objects
    {
        Victory, Lose
    }

    private enum Texts
    {
        Text_Result
    }

    private enum Buttons
    {
        // Victory
        Button_FullScreen,

        // Lose
        Button_Exit, Button_Retry, Button_Draw, Button_Enhance
    }
}
