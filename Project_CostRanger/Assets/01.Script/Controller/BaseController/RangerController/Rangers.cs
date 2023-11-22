using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public abstract class Ranger 
{
    protected RangerController controller;
    public List<Define.Specialty> specialties;
    protected WaitForSeconds attackWaitForSeceonds;
    protected WaitForSeconds skillWaitForSeconds;

    public virtual void AddAnimationHash()
    {
        controller.animationHash.Add(RangerState.Idle, Animator.StringToHash("0_idle"));
        controller.animationHash.Add(RangerState.Follow, Animator.StringToHash("1_Run"));
        controller.animationHash.Add(RangerState.Attack, Animator.StringToHash("2_Attack_Normal"));
        controller.animationHash.Add(RangerState.Die, Animator.StringToHash("4_Death"));
        controller.animationHash.Add(RangerState.SkillCast, Animator.StringToHash("5_Skill_Normal"));
    }

    public virtual bool CheckFollow()
    {
        if (controller.attackTarget == null)
            controller.FindAttackTarget();

        if (Vector2.Distance(controller.attackTarget.transform.position, controller.transform.position) > controller.status.CurrentAttackDistance)
        {
            controller.ChangeState(Define.RangerState.Follow);
            return true;
        }

        controller.Stop();
        return false;
    }

    //추적
    public virtual void Follow()
    {
        if (controller.attackTarget == null)
            controller.FindAttackTarget();

        Vector2 dir = (controller.attackTarget.transform.position - controller.transform.position).normalized;
        controller.rb.velocity = dir * controller.status.CurrentMoveSpeed * Time.fixedDeltaTime * 10;
    }

    public virtual void CheckAttackCooltime()
    {
        //공격 쿨타임이 있을 때 쿨타임 감소
        if (controller.status.CheckAttackCooltime > 0)
        {
            controller.status.CheckAttackCooltime -= Time.deltaTime;
            if (controller.status.CheckAttackCooltime <= 0)
                controller.status.CheckAttackCooltime = 0;
        }
    }

    //공격 가능한지 체크
    public virtual bool CheckAttack()
    {
        //예외 처리
        if (controller.attackTarget == null) return false;
        if (controller.status.CheckAttackCooltime > 0) return false;

        //공격 가능한 거리인지 체크 후 가능하면 공격 상태로 변환
        if (Vector2.Distance(controller.attackTarget.transform.position, controller.transform.position) <= controller.status.CurrentAttackDistance)
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
        controller.Stop();
        controller.status.CheckAttackCooltime = controller.status.CurrentAttackSpeed;
        //Managers.Battle.AttackCalculation(controller, controller.attackTarget, (_damage) => { /*controller.mvpPoint += _damage;*/ });
        //Managers.Game.battleInfo.UpdateMVPPoints();
        yield return attackWaitForSeceonds; //애니메이션 시간 기다리는 거임
        controller.ChangeState(Define.RangerState.Idle);
        controller.routines.Remove("attack");
    }
}

namespace Rangers
{
    public class TestRanger : Ranger
    {
        public TestRanger(RangerController _controller)
        {
            controller = _controller;
            attackWaitForSeceonds = new WaitForSeconds(Define.normalAttackAnimationTime);
            skillWaitForSeconds = new WaitForSeconds(Define.skillAnimationTime);
            specialties = new List<Define.Specialty>();
        }
    }
}