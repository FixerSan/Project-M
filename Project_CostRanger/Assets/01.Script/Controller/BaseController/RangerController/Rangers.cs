using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public abstract class Ranger 
{
    protected RangerController controller;
    public List<Define.SpecialtyType> specialties;
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
        if (controller.attackTarget == null || controller.attackTarget.currentState == EnemyState.Die)
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
        if (controller.attackTarget == null || controller.attackTarget.currentState == EnemyState.Die)
            controller.FindAttackTarget();

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
        Managers.Battle.AttackCalculation(controller, controller.attackTarget);
        yield return attackWaitForSeceonds; //애니메이션 시간 기다리는 거임
        controller.status.CheckAttackCooltime = controller.status.CurrentAttackSpeed;
        controller.ChangeState(Define.RangerState.Idle);
        controller.routines.Remove("attack");
    }

    public virtual void CheckSkillCooltime()
    {
        if (controller.status.CheckSkillCooltime > 0)
        {
            controller.status.CheckSkillCooltime -= Time.deltaTime;
            if (controller.status.CheckSkillCooltime <= 0)
                controller.status.CheckSkillCooltime = 0;
        }
    }

    public virtual void UseSkill()
    {
        if (controller.status.CheckSkillCooltime == 0)
        {
            controller.ChangeState(RangerState.SkillCast);
        }
    }

    public virtual bool CheckCanUseSkill()
    {
        if (!Managers.Game.battleStageSystem.isAutoSkill) return false;
        if (controller.status.CheckSkillCooltime == 0)
        {
            controller.ChangeState(RangerState.SkillCast);
            return true;
        }

        return false;
    }

    public virtual void Skill()
    {
        controller.routines.Add("skill", controller.StartCoroutine(SkillRoutine()));
    }

    //스킬 처리 루틴
    public virtual IEnumerator SkillRoutine()
    {
        controller.Stop();
        Debug.Log("스킬 사용됨");
        yield return skillWaitForSeconds; //애니메이션 시간 기다리는 거임
        controller.ChangeState(Define.RangerState.Idle);
        controller.status.CheckSkillCooltime = controller.status.CurrentSkillCooltime;
        controller.routines.Remove("skill");
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
            specialties = new List<Define.SpecialtyType>();
        }
    }
}