using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ranger 
{
    private RangerController controller;
    public Define.Specialty[] specialty;
    protected WaitForSeconds attackWaitForSeceonds;
    protected WaitForSeconds skillWaitForSeconds;


    //추적
    public virtual void Follow()
    {
        if (controller.attackTarget != null)
        {
            if (Vector2.Distance(controller.attackTarget.transform.position, controller.transform.position) < controller.status.CurrentAttackDistance)
                return;
            Vector2 dir = (controller.attackTarget.transform.position - controller.transform.position).normalized;
            controller.transform.Translate((dir * controller.status.CurrentMoveSpeed) * Time.deltaTime);
        }
    }

    //공격 가능한지 체크
    public virtual bool CheckAttack()
    {
        //공격 쿨타임이 있을 때 쿨타임 감소
        if (controller.status.CheckAttackCooltime > 0)
        {
            controller.status.CheckAttackCooltime -= Time.deltaTime;
            if (controller.status.CheckAttackCooltime <= 0)
                controller.status.CheckAttackCooltime = 0;
        }

        //예외 처리
        if (controller.attackTarget == null) return false;
        if (controller.status.CheckAttackCooltime > 0) return false;
        if (controller.currentState != Define.RangerState.Follow) return false;

        //공격 가능한 거리인지 체크 후 가능하면 공격 상태로 변환
        if (Vector2.Distance(controller.attackTarget.transform.position, controller.transform.position) < controller.status.CurrentAttackDistance)
        {
            controller.ChangeState(Define.RangerState.Attack);
            return true;
        }
        return false;
    }

    //공격 처리
    public virtual void Attack()
    {
        controller.routines.Add("attack", controller.StartCoroutine(AttackRoutine()));
    }

    //공격 처리 루틴
    public virtual IEnumerator AttackRoutine()
    {
        controller.status.CheckAttackCooltime = controller.status.CurrentAttackSpeed;
        Managers.Battle.AttackCalculation(controller, controller.attackTarget, (_damage) => { /*controller.mvpPoint += _damage;*/ });
        //Managers.Game.battleInfo.UpdateMVPPoints();
        yield return attackWaitForSeceonds; //애니메이션 시간 기다리는 거임
        controller.ChangeState(Define.RangerState.Follow);
        controller.routines.Remove("attack");
    }
}

namespace Rangers
{
    public class TestRanger : Ranger
    {
        public TestRanger()
        {
            attackWaitForSeceonds = new WaitForSeconds(1);
        }
    }
}