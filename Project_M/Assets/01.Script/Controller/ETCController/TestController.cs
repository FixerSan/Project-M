using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            Managers.Scene.LoadScene(Define.Scene.Stage);
        if (Input.GetKeyDown(KeyCode.U))
            Managers.UI.ShowPopupUI<UIPopup_BattleBefore>("UIBattleBefore");
    }
}
