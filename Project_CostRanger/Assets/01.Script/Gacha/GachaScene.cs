using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaScene : UIScene
{
    // 가챠 시스템 연결

    public override bool Init()
    {
        if (!base.Init())
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        // BindEvent(GetButton((int)Buttons.Button_Login).gameObject, _callback: OnClick_Login);
        // BindEvent(GetButton((int)Buttons.Button_SignUp).gameObject, _callback: OnClick_SignUp);
        // GetText((int)Texts.Text_NotExistPlayerData).gameObject.SetActive(false);
        // GetText((int)Texts.Text_IncorrectPassward).gameObject.SetActive(false);
        return true;
    }

    public void OnClick_TryGacha()
    {
        GetText((int)Texts.Text_ConfirmationMessage).gameObject.SetActive(true);

        // Managers.Gacha.TryGacha(1);
    }

    public void OnClick_TryGachaTenTimes()
    {
        // Managers.Gacha.TryGacha(10)

        Managers.UI.ShowPopupUI<UIPopup_SignUp>();
    }

    private enum Buttons
    {
        Button_TryGacha, Button_TryGachaTenTimes
    }

    private enum Texts
    {
        Text_ConfirmationMessage
    }
}
