using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_SignUp : UIPopup
{
    public override bool Init()
    {
        if (!base.Init())
            return false;
        BindButton(typeof(Buttons));
        BindInputField(typeof(InputFields));

        BindEvent(GetButton((int)Buttons.Button_SignUp_Close).gameObject, _callback: OnClick_SignUp_Close);
        BindEvent(GetButton((int)Buttons.Button_SignUp_Complete).gameObject, _callback: OnClick_SignUp_Complete);
        return true;
    }

    public void OnClick_SignUp_Close()
    {
        ClosePopupUP();
    }

    public void OnClick_SignUp_Complete()
    {
        Managers.Game.SignUp(GetInputField((int)InputFields.InputField_SignUp_ID).text, GetInputField((int)InputFields.InputField_SignUp_NickName).text, GetInputField((int)InputFields.InputField_SignUp_PW).text, GetInputField((int)InputFields.InputField_SignUp_PWReCheck).text, (_signEvent) =>
        {
            Debug.Log(_signEvent);
            if (_signEvent == Define.SignUpEvent.ExistSameID)
            {

                return;
            }

            if (_signEvent == Define.SignUpEvent.PasswardNotSame)
            {

                return;
            }

            if (_signEvent == Define.SignUpEvent.SuccessSignUp)
            {
                OnClick_SignUp_Close();
            }
        });
    }

    public override void RedrawUI()
    {

    }

    private enum InputFields
    {
        InputField_SignUp_ID, InputField_SignUp_NickName, InputField_SignUp_PW, InputField_SignUp_PWReCheck
    }

    private enum Buttons
    {
         Button_SignUp_Close, Button_SignUp_Complete
    }
}
