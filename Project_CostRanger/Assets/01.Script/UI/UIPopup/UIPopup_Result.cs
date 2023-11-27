using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_Result : UIPopup
{
    public void Init(Define.GameResult _result, int _resultUID)
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        BindEvent(GetButton((int)Buttons.Button_Home).gameObject, OnClick_Home);

        if (_result == Define.GameResult.Victory) Victory();
        if (_result == Define.GameResult.Lose) Lose();
    }

    public void Victory()
    {
        GetText((int)Texts.Text_Result).text = "Victory";
    }

    public void Lose()
    {
        GetText((int)Texts.Text_Result).text = "Lose";
    }

    public void OnClick_Home()
    {
        Managers.Scene.LoadScene(Define.Scene.Main);
    }

    private enum Texts
    {
        Text_Result
    }

    private enum Buttons
    {
        Button_Home
    }
}
