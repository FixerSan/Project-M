using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScene_Login: UIScene
{
    public override bool Init()
    {
        if (!base.Init())
            return false;
        //BindButton(typeof(Buttons));
        //BindInputField(typeof(InputFields));
        //BindText(typeof(Texts));

        BindEvent(gameObject, _callback: OnClick_FixerLogin);
        //BindEvent(GetButton((int)Buttons.Button_Login).gameObject, _callback: OnClick_Login);
        //BindEvent(GetButton((int)Buttons.Button_SignUp).gameObject, _callback: OnClick_SignUp);
        //BindEvent(GetButton((int)Buttons.Button_FixerLogin).gameObject, _callback: OnClick_FixerLogin);
        //GetText((int)Texts.Text_NotExistPlayerData).gameObject.SetActive(false);
        //GetText((int)Texts.Text_IncorrectPassward).gameObject.SetActive(false);
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
        Managers.UI.ShowPopupUI<UIPopup_SignUp>();
    }

    public void OnClick_FixerLogin()
    {
        Managers.Game.Login("Fixer1302", "1234", (_loginEvent) =>
        {
            if (_loginEvent == Define.LoginEvent.NotExistPlayerData)
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

    private enum Buttons
    {
        Button_Login, Button_SignUp, Button_FixerLogin
    }    

    private enum InputFields
    {
        InputField_ID, InputField_PW
    }

    private enum Texts
    {
        Text_NotExistPlayerData, Text_IncorrectPassward
    }

}
