using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager
{
    ////���� ó��
    //public void AttackCalculation(BaseController _attacker, BaseController _hiter, Action<float> _damageCallback = null)
    //{
    //    float currentDamage = _attacker.status.CurrentAttackForce;

    //    //ġ��Ÿ���� üũ �� ������ ġ��Ÿ ó��
    //    float tempInt = UnityEngine.Random.Range(0, 101);
    //    if(tempInt >= _attacker.status.CurrentCriticalProbability)
    //        currentDamage = (int)(currentDamage * _attacker.status.CurrentCriticalForce);

    //    //������ ó�� �� �ݹ�
    //    _hiter.Hit(currentDamage);
    //    _damageCallback?.Invoke(currentDamage);
    //}

    ///// <summary>
    ///// ���� ó���� ��ġ�� �� �� ó��
    ///// </summary>
    ///// <param name="_damage">������</param>
    ///// <param name="_hiter">�´� ���</param>
    ///// <param name="_damageCallback">�ݹ�</param>
    //public void AttackCalculation(int _damage, BattleEntityController _hiter, Action<int> _damageCallback = null)
    //{
    //    //int tempInt = UnityEngine.Random.Range(0, 101);
    //    //int currentDamage = _damage;

    //    //if (tempInt > 50)
    //    //    currentDamage = (int)(currentDamage * criticalMultiplier);

    //    //_hiter.Hit(currentDamage);
    //    //_damageCallback?.Invoke(currentDamage);
    //}
}
