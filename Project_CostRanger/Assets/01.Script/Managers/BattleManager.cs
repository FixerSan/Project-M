using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager
{
    private bool isCritical = false;
    ////공격 처리
    public void AttackCalculation(BaseController _attacker, BaseController _hiter, float _damage = -1 ,Action<float> _damageCallback = null)
    {
        isCritical = false;
        float currentDamage = _damage;
        if (_damage == -1)
            currentDamage = _attacker.status.CurrentAttackForce;

        //치명타인지 체크 후 맞으면 치명타 처리
        float tempInt = UnityEngine.Random.Range(0, 101);
        if (tempInt <= _attacker.status.CurrentCriticalProbability)
        {
            isCritical = true;
            currentDamage = (int)(currentDamage * _attacker.status.CurrentCriticalForce);
        }

        //데미지 처리 및 콜백
        _hiter.Hit(currentDamage);
        //Managers.UI.MakeWorldText($"{_damage}", _hiter.worldTextTrans.position, Define.TextType.Damage);
        _damageCallback?.Invoke(currentDamage);
    }
}
