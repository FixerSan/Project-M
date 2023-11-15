using System;
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
        GetText((int)Texts.Text_Success).gameObject.SetActive(false);
        GetText((int)Texts.Text_Fail).gameObject.SetActive(false);
        return true;
    }

    private void OnClick_CreateButton()
    {
        Managers.Stage.CreateStage(GetInputField((int)InputFields.InputField_StageUID).text, GetInputField((int)InputFields.InputField_StageName).text, GetInputField((int)InputFields.InputField_CanUseCost).text,
                                   GetInputField((int)InputFields.InputField_EnemyOne).text, GetInputField((int)InputFields.InputField_EnemyTwo).text, GetInputField((int)InputFields.InputField_EnemyThree).text,
                                   GetInputField((int)InputFields.InputField_EnemyFour).text, GetInputField((int)InputFields.InputField_EnemyFive).text, GetInputField((int)InputFields.InputField_EnemySix).text,
                                   GetInputField((int)InputFields.InputField_EnemySeven).text, GetInputField((int)InputFields.InputField_EnemyEight).text, GetInputField((int)InputFields.InputField_EnemyNine).text, RedrawUI);
    }

    //UI 다시 그리기
    private void RedrawUI(Define.CreateStageEvent _eventType)
    {
        //초기화
        GetText((int)Texts.Text_Success).gameObject.SetActive(false);
        GetText((int)Texts.Text_Fail).gameObject.SetActive(false);

        //UID를 입력하지 않았을 때
        if(_eventType == Define.CreateStageEvent.NotInputUID)
        {
            GetText((int)Texts.Text_Fail).gameObject.SetActive(true);
            GetText((int)Texts.Text_Fail).text = "You are not Input Stage UID";
            return;
        }

        //입력한 UID가 Int가 아닐 때
        if (_eventType == Define.CreateStageEvent.UIDIsNotInt)
        {
            GetText((int)Texts.Text_Fail).gameObject.SetActive(true);
            GetText((int)Texts.Text_Fail).text = "UID is not Int";
            return;
        }

        //입력한 UID가 이미 존재할 때
        if (_eventType == Define.CreateStageEvent.ExistSameUID)
        {
            GetText((int)Texts.Text_Fail).gameObject.SetActive(true);
            GetText((int)Texts.Text_Fail).text = "UID already exist";
            return;
        }

        //이름을 입력하지 않았을 때
        if (_eventType == Define.CreateStageEvent.NotInputName)
        {
            GetText((int)Texts.Text_Fail).gameObject.SetActive(true);
            GetText((int)Texts.Text_Fail).text = "You are not Input Stage name";
            return;
        }

        //사용 가능한 코스트를 입력하지 않았을 때
        if (_eventType == Define.CreateStageEvent.NotInputCanUseCost)
        {
            GetText((int)Texts.Text_Fail).gameObject.SetActive(true);
            GetText((int)Texts.Text_Fail).text = "You are not Input Stage Cost";
            return;
        }

        //코스트가 인트가 아닐 때 
        if (_eventType == Define.CreateStageEvent.CostIsNotInt)
        {
            GetText((int)Texts.Text_Fail).gameObject.SetActive(true);
            GetText((int)Texts.Text_Fail).text = "Cost is not Int";
            return;
        }

        //성공
        if (_eventType == Define.CreateStageEvent.SuccessCreate)
        {
            GetText((int)Texts.Text_Success).gameObject.SetActive(true);
            return;
        }
    }

    private enum InputFields
    {
        InputField_EnemyOne, InputField_EnemyTwo, InputField_EnemyThree, InputField_EnemyFour, InputField_EnemyFive, InputField_EnemySix, InputField_EnemySeven, InputField_EnemyEight, InputField_EnemyNine,
        InputField_StageUID, InputField_StageName, InputField_CanUseCost
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
