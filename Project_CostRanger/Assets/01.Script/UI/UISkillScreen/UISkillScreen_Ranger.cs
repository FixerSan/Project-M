using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISkillScreen_Ranger : UIBase
{
    public override bool Init()
    {
        if(!base.Init()) return false;
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        return true;
    }

    public void ApplyRangerSkill(RangerInfoData _data, Action _callback = null)
    {
        StartCoroutine(ApplyRangerSkillRoutine(_data, _callback));
    }

    public IEnumerator ApplyRangerSkillRoutine(RangerInfoData _data, Action _callback = null)
    {
        TMP_Text text = GetText((int)Texts.Text_Sentence);
        text.text = "Blah Blah";

        Managers.Resource.Load<Sprite>(_data.name, (_sprite) => 
        {
            GetImage((int)Images.Image_Illust).sprite = _sprite;
        });
        yield return new WaitForSecondsRealtime(Define.skillScreenTime);
        _callback.Invoke();
    }

    private enum Texts
    {
        Text_Sentence
    }

    private enum Images
    {
        Image_Illust
    }
}
