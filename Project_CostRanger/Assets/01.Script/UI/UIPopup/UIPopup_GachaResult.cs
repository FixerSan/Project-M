using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_GachaResult : UIPopup
{
    public UISlot_GachaInfo[] gachaInfoSlots;

    public override bool Init()
    {
        if (!base.Init())
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        BindEvent(GetButton((int)Buttons.Button_Next).gameObject, _callback: OnClick_Next);

        RedrawGachaSlot(Managers.Gacha.GetGachaResult());

        return true;
    }

    public void RedrawGachaSlot(int[] _obtainedList)
    {
        foreach (var slot in gachaInfoSlots)
        {
            slot.gameObject.SetActive(false);
        }

        for (int i = 0; i < _obtainedList.Length; i++)
        {
            if (_obtainedList[i] != 0)
            {
                bool isAlreadyObtained = false;

                //for (int j = 0, jmax = Managers.Data.playerData.hasRangers.Count; j < jmax; i++)
                //{
                //    if (Managers.Data.playerData.hasRangers[i] != null && Managers.Data.playerData.hasRangers[i].UID == _obtainedList[i])
                //    {
                //        isAlreadyObtained = true;
                //        break;
                //    }
                //}

                gachaInfoSlots[i].Redraw(_obtainedList[i], isAlreadyObtained);
            }
        }
    }

    public void OnClick_Next()
    {
        Managers.UI.ClosePopupUI(this);
    }

    public enum Buttons
    {
        Button_Next
    }

    public enum Texts
    {

    }
}
