using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_Login: UIPopup
{
    public override bool Init()
    {
        if (!base.Init())
            return false;
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        BindInputField(typeof(InputFields));

        BindEvent(GetButton((int)Buttons.Button_Login).gameObject, _callback: OnClick_Login);
        return true;
    }

    public void OnClick_Login()
    {
        Managers.Game.Login(GetInputField((int)InputFields.InputField_ID).text, GetInputField((int)InputFields.InputField_PW).text, (_loginEvent) => 
        {
            Debug.Log(_loginEvent);
        });
    }

    private enum Objects
    {

    }

    private enum Buttons
    {
        Button_Login
    }    

    private enum InputFields
    {
        InputField_ID, InputField_PW
    }

}
