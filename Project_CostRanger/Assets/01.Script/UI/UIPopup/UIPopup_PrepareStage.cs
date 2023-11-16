using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_PrepareStage : UIPopup
{
    public override bool Init()
    {
        Managers.UI.SetCanvas(gameObject, true);

        BindButton(typeof(Buttons));




        return true;
    }

    private enum Buttons
    {

    }
}
