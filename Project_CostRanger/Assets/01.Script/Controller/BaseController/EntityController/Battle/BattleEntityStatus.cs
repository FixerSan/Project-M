using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntityStatus 
{
    private BattleEntityController controller;
    public int maxHP;
    private int currentHP;
    public int CurrentHP 
    {
        get { return currentHP; }
        set 
        { 
            currentHP = value;
            Managers.Event.OnVoidEvent?.Invoke(Define.VoidEventType.OnChangeControllerStatus);
        }
    }
    public int attackForce;
    public float skillCooltime;
    public float currentSkillCooltime;
    public int moveSpeed;

    public int currentAttackForce;
    public float currentAttackCycle;
    public float checkAttackTime;

    public BattleBuff buff;

    public BattleEntityStatus(BattleEntityController _controller, int _maxHP, int _currentHP, int _attackForce,  float _skillCooltime, float _currentSkillCooltime, int _moveSpeed, float _attackCycle)
    {
        controller = _controller;
        maxHP = _maxHP;
        currentHP = _currentHP; 
        attackForce = _attackForce;
        currentAttackForce = _attackForce;
        currentAttackCycle = _attackCycle;
        skillCooltime = _skillCooltime;
        checkAttackTime = 0;
        currentSkillCooltime = _currentSkillCooltime;
        moveSpeed = _moveSpeed;
        

        buff = new BattleBuff(controller, this);
    }
}
