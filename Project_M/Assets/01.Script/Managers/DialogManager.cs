using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager
{
    public Action callback;

    private UIPopup_DialogSpeaker speaker;    // 다이얼로그 스피커 선언
    public UIPopup_DialogSpeaker Speaker      // 다이얼로그 스피커 프로퍼티 선언
    {
        get
        {
            if(speaker == null)
            {
                speaker = Managers.UI.ShowPopupUI<UIPopup_DialogSpeaker>("UIPopup_DialogSpeaker", true);
                speaker.Init();
            }
            return speaker;
        }
    }
    private DialogData currentData;     //현제 다이얼로그 데이터

    // 다이얼로그 불러오기
    public void Call(int _dialogIndex, Action _callback = null)
    {
        currentData = Managers.Data.GetDialogData(_dialogIndex);
        Speaker.ApplyDialog(currentData);
        if(_callback != null) callback = _callback;
    }

    // 다이얼로그 버튼 사운드 호출
    private void PlayBtnSound()
    {
        //Managers.Sound.PlaySoundEffect(Define.AudioClip_Effect.Dialog_Next);
    }

    // 다이얼로그 버튼 1 처리 코드
    public void OnClick_ButtonOne()
    {
        PlayBtnSound();
        if (currentData.nextDialogUID == -100)  { EndDialog();  return; }
        if (currentData.nextDialogUID != -1) { Call(currentData.nextDialogUID, callback); return; }

        switch (currentData.dialogUID)
        {
            case 0:

                break;

            default:
                EndDialog();
                return;
        }
    }

    // 다이얼로그 버튼 2 처리 코드
    public void OnClick_ButtonTwo()
    {
        PlayBtnSound();
        if (currentData.nextDialogUID == -100) { EndDialog(); return; }
        if (currentData.nextDialogUID != -1) { Call(currentData.nextDialogUID, callback); return; }

        switch (currentData.dialogUID)
        {
            case 0:

                break;
        }
    }

    // 다이얼로그 버튼 3 처리 코드
    public void OnClick_ButtonThree()
    {
        PlayBtnSound();
        if (currentData.nextDialogUID == -100) { EndDialog(); return; }
        if (currentData.nextDialogUID != -1) { Call(currentData.nextDialogUID, callback); return; }

        switch (currentData.dialogUID)
        {
            case 0:

                break;
        }
    }

    // 다이얼로그 종료
    public void EndDialog()
    {
        Speaker.CloseDialog();
        speaker = null;
        callback?.Invoke();
        callback = null;
    }
}
