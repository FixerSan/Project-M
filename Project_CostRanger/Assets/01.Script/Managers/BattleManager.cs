using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager
{
    ////���� ó��
    public void AttackCalculation(BaseController _attacker, BaseController _hiter, float _damage = -1 ,Action<float> _damageCallback = null)
    {
        float currentDamage = _damage;
        if (_damage == -1)
            currentDamage = _attacker.status.CurrentAttackForce;

        //ġ��Ÿ���� üũ �� ������ ġ��Ÿ ó��
        float tempInt = UnityEngine.Random.Range(0, 100);
        if (tempInt < _attacker.status.CurrentCriticalProbability)
        {
            currentDamage = (int)(currentDamage * _attacker.status.CurrentCriticalForce);
            if(_attacker is RangerController)
            {
                RangerController ranger = _attacker as RangerController;
            }
        }


        //���� ���
        currentDamage = currentDamage * (100 / (_hiter.status.CurrentDefenseForce + 100));

        //������ ó�� �� �ݹ�
        _hiter.Hit(currentDamage);
        //Managers.UI.MakeWorldText($"{_damage}", _hiter.worldTextTrans.position, Define.TextType.Damage);
        _damageCallback?.Invoke(currentDamage);
    }
}
