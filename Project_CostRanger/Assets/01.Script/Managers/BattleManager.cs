using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager
{
    //ġ��Ÿ ����
    public readonly float criticalMultiplier = 2f;

    //���� ó��
    public void AttackCalculation(BattleEntityController _attacker, BattleEntityController _hiter, Action<int> _damageCallback = null)
    {
        int tempInt = UnityEngine.Random.Range(0, 101);
        int currentDamage = _attacker.status.attackForce;

        //�������� 50�� ������ ġ��Ÿ ������ ���
        if(tempInt > 50)
            currentDamage = (int)(currentDamage * criticalMultiplier);

        //������ ó�� �� �ݹ�
        _hiter.Hit(currentDamage);
        _damageCallback?.Invoke(currentDamage);
    }

    /// <summary>
    /// ���� ó���� ��ġ�� �� �� ó��
    /// </summary>
    /// <param name="_damage">������</param>
    /// <param name="_hiter">�´� ���</param>
    /// <param name="_damageCallback">�ݹ�</param>
    public void AttackCalculation(int _damage, BattleEntityController _hiter, Action<int> _damageCallback = null)
    {
        int tempInt = UnityEngine.Random.Range(0, 101);
        int currentDamage = _damage;

        if (tempInt > 50)
            currentDamage = (int)(currentDamage * criticalMultiplier);

        _hiter.Hit(currentDamage);
        _damageCallback?.Invoke(currentDamage);
    }
}
