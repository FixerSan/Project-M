using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleBuff 
{
    public BattleEntityController controller;
    public BattleEntityStatus status;
    public bool isPlusAttackSpeed;
    public float currentPlusAttackSpeedTime;
    public float plusAttackSpeed;
    public bool isCanMiss;
    public int canMissCount;
    public float checkTime;

    public void CheckBuff()
    {
        CheckPlusAttackSpeed();
    }

    public void CheckPlusAttackSpeed()
    {
        if (!isPlusAttackSpeed) return;

        currentPlusAttackSpeedTime -= Time.deltaTime;
        if (currentPlusAttackSpeedTime <= 0)
        {
            EndPlusAttackSpeed();
        }
    }

    public void StartPlusAttackSpeed(float _time, float _plusAttackSpeed)
    {
        isPlusAttackSpeed = true;
        currentPlusAttackSpeedTime = _time;
        plusAttackSpeed = _plusAttackSpeed;
        status.currentAttackCycle -= _plusAttackSpeed;
    }

    public void EndPlusAttackSpeed()
    {
        isPlusAttackSpeed = false;
        currentPlusAttackSpeedTime = 0;

        status.currentAttackCycle += plusAttackSpeed;
        plusAttackSpeed = 0;
    }

    public void SetMissCount(int _count)
    {
        isCanMiss = true;
        canMissCount += _count;
        Managers.UI.MakeWorldText("MISS Count + 3", controller.transform.position + controller.textOffset, Define.TextType.Normal);
    }

    public bool CheckCanMiss()
    {
        if(isCanMiss)
        {
            canMissCount--;
            if (canMissCount <= 0)
                isCanMiss = false;
        }
        return isCanMiss;
    }

    public BattleBuff(BattleEntityController _controller, BattleEntityStatus _status)
    {
        controller = _controller;
        status = _status;
        isPlusAttackSpeed = false;
        currentPlusAttackSpeedTime = 0;
        plusAttackSpeed = 0;
        isCanMiss = false; 
        canMissCount = 0;
        checkTime = 0; ;
    }
}
