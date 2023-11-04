using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager
{
    public Action callback;

    private UIPopup_DialogSpeaker speaker;    // ���̾�α� ����Ŀ ����
    public UIPopup_DialogSpeaker Speaker      // ���̾�α� ����Ŀ ������Ƽ ����
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
    private DialogData currentData;     //���� ���̾�α� ������

    // ���̾�α� �ҷ�����
    public void Call(int _dialogIndex, Action _callback = null)
    {
        currentData = Managers.Data.GetDialogData(_dialogIndex);
        Speaker.ApplyDialog(currentData);
        if(_callback != null) callback = _callback;
    }

    // ���̾�α� ��ư ���� ȣ��
    private void PlayBtnSound()
    {
        //Managers.Sound.PlaySoundEffect(Define.AudioClip_Effect.Dialog_Next);
    }

    // ���̾�α� ��ư 1 ó�� �ڵ�
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

    // ���̾�α� ��ư 2 ó�� �ڵ�
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

    // ���̾�α� ��ư 3 ó�� �ڵ�
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

    // ���̾�α� ����
    public void EndDialog()
    {
        Speaker.CloseDialog();
        speaker = null;
        callback?.Invoke();
        callback = null;
    }
}
