using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScene_CreateStage : UIScene
{
    public override bool Init()
    {
        if (!base.Init()) return false;
        BindInputField(typeof(InputFields));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        BindEvent(GetButton((int)Buttons.Button_Create).gameObject, OnClick_CreateButton);
        return true;
    }

    private void OnClick_CreateButton()
    {
        //Managers.Stage.CreateStage(GetInputField((int)InputFields.InputField_StageName).text, );
    }

    private enum InputFields
    {
        InputField_EnemyOne, InputField_EnemyTwo, InputField_EnemyThree, InputField_EnemyFour, InputField_EnemyFive, InputField_EnemySix, InputField_EnemySeven, InputField_EnemyEight, InputField_EnemyNine,
        InputField_StageName, InputField_CanUseCost, InputField_ClearReward, InputField_OneStarReward, InputField_TwoStarReward, InputField_ThreeStarReward
    }

    private enum Texts
    {
        Text_Success, Text_Fail
    }

    private enum Buttons
    {
        Button_Create
    }
}
