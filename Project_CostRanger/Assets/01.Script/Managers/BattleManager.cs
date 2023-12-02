using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager
{
    private bool isCritical = false;
    ////���� ó��
    public void AttackCalculation(BaseController _attacker, BaseController _hiter, float _damage = -1 ,Action<float> _damageCallback = null)
    {
        isCritical = false;
        float currentDamage = _damage;
        if (_damage == -1)
            currentDamage = _attacker.status.CurrentAttackForce;

        //ġ��Ÿ���� üũ �� ������ ġ��Ÿ ó��
        float tempInt = UnityEngine.Random.Range(0, 101);
        if (tempInt <= _attacker.status.CurrentCriticalProbability)
        {
            isCritical = true;
            currentDamage = (int)(currentDamage * _attacker.status.CurrentCriticalForce);
        }

        //������ ó�� �� �ݹ�
        _hiter.Hit(currentDamage);
        //Managers.UI.MakeWorldText($"{_damage}", _hiter.worldTextTrans.position, Define.TextType.Damage);
        _damageCallback?.Invoke(currentDamage);
    }
}
