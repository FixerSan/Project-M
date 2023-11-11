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
        BindText(typeof(Texts));

        BindEvent(GetButton((int)Buttons.Button_Login).gameObject, _callback: OnClick_Login);
        BindEvent(GetButton((int)Buttons.Button_SignUp).gameObject, _callback: OnClick_SignUp);
        BindEvent(GetButton((int)Buttons.Button_SignUp_Close).gameObject, _callback: OnClick_SignUp_Close);
        BindEvent(GetButton((int)Buttons.Button_SignUp_Complete).gameObject, _callback: OnClick_SignUp_Complete);
        GetText((int)Texts.Text_NotExistPlayerData).gameObject.SetActive(false);
        GetText((int)Texts.Text_IncorrectPassward).gameObject.SetActive(false);
        GetObject((int)Objects.Bundle_SignUp).SetActive(false);
        return true;
    }

    public void OnClick_Login()
    {
        GetText((int)Texts.Text_NotExistPlayerData).gameObject.SetActive(false);
        GetText((int)Texts.Text_IncorrectPassward).gameObject.SetActive(false);

        Managers.Game.Login(GetInputField((int)InputFields.InputField_ID).text, GetInputField((int)InputFields.InputField_PW).text, (_loginEvent) => 
        {
            if(_loginEvent == Define.LoginEvent.NotExistPlayerData)
            {
                GetText((int)Texts.Text_NotExistPlayerData).gameObject.SetActive(true);
            }

            if (_loginEvent == Define.LoginEvent.IncorrectPassward)
            {
                GetText((int)Texts.Text_IncorrectPassward).gameObject.SetActive(true);
            }

            if (_loginEvent == Define.LoginEvent.SuccessLogin)
            {
                Managers.Scene.LoadScene(Define.Scene.Main);
            }
        });
    }

    public void OnClick_SignUp()
    {
        GetObject((int)Objects.Bundle_SignUp).SetActive(true);
    }

    public void OnClick_SignUp_Close()
    {
        GetObject((int)Objects.Bundle_SignUp).SetActive(false);
    }

    public void OnClick_SignUp_Complete()
    {
        Managers.Game.SignUp(GetInputField((int)InputFields.InputField_SignUp_ID).text, GetInputField((int)InputFields.InputField_SignUp_NickName).text, GetInputField((int)InputFields.InputField_SignUp_PW).text, GetInputField((int)InputFields.InputField_SignUp_PWReCheck).text, (_signEvent) => 
        {
            Debug.Log(_signEvent);
            if(_signEvent == Define.SignUpEvent.ExistSameID)
            {

                return;
            }

            if(_signEvent == Define.SignUpEvent.PasswardNotSame)
            {

                return;
            }

            if(_signEvent == Define.SignUpEvent.SuccessSignUp)
            {
                OnClick_SignUp_Close();
            }
        });
    }

    private enum Objects
    {
        Bundle_SignUp
    }

    private enum Buttons
    {
        Button_Login, Button_SignUp, Button_SignUp_Close, Button_SignUp_Complete
    }    

    private enum InputFields
    {
        InputField_ID, InputField_PW, InputField_SignUp_ID, InputField_SignUp_NickName, InputField_SignUp_PW, InputField_SignUp_PWReCheck
    }

    private enum Texts
    {
        Text_NotExistPlayerData, Text_IncorrectPassward
    }

}
