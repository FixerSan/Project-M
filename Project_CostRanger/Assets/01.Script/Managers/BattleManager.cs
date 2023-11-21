using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager
{
    ////공격 처리
    //public void AttackCalculation(BaseController _attacker, BaseController _hiter, Action<float> _damageCallback = null)
    //{
    //    float currentDamage = _attacker.status.CurrentAttackForce;

    //    //치명타인지 체크 후 맞으면 치명타 처리
    //    float tempInt = UnityEngine.Random.Range(0, 101);
    //    if(tempInt >= _attacker.status.CurrentCriticalProbability)
    //        currentDamage = (int)(currentDamage * _attacker.status.CurrentCriticalForce);

    //    //데미지 처리 및 콜백
    //    _hiter.Hit(currentDamage);
    //    _damageCallback?.Invoke(currentDamage);
    //}

    ///// <summary>
    ///// 공격 처리를 수치로 할 때 처리
    ///// </summary>
    ///// <param name="_damage">데미지</param>
    ///// <param name="_hiter">맞는 대상</param>
    ///// <param name="_damageCallback">콜백</param>
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
