using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillScreen_Enemy : UIBase
{
    public override bool Init()
    {
        if (!base.Init()) return false;
        BindImage(typeof(Images));
        BindText(typeof(Images));
        return true;
    }

    public void ApplyEnemySkill(EnemyInfoData _enemyData, Action _callback = null)
    {
        StartCoroutine(ApplyEnemySkillRoutine(_enemyData, _callback));
    }

    public IEnumerator ApplyEnemySkillRoutine(EnemyInfoData _enemyData, Action _callback = null)
    {
        yield return new WaitForSeconds(Define.skillScreenTime);
        _callback.Invoke();
    }

    private enum Texts
    {

    }

    private enum Images
    {

    }
}
