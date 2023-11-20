using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScene : BaseScene
{
    private WaitForSeconds sceneStartDelay;
    
    public override void Init(Action _callback)
    {
        sceneStartDelay = new WaitForSeconds(Define.sceneStartDelay);

        Managers.UI.ShowSceneUI<UIScene_Stage>();
    }
    

    public override void SceneEvent(int _eventIndex, Action _callback = null)
    {
        switch (_eventIndex)
        {
            case 0:
                break;

            case 1:
                break;
        }
        _callback?.Invoke();
    }
    
    public IEnumerator SceneEventOneRoutine()
    {
        yield return sceneStartDelay;
    }

    public override void Clear()
    {

    }


    private enum Trans
    {

    }
}
