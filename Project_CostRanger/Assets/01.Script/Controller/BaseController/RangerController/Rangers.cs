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

    //����
    public virtual void Follow()
    {
        if (controller.attackTarget == null)
            controller.FindAttackTarget();

        Vector2 dir = (controller.attackTarget.transform.position - controller.transform.position).normalized;
        controller.rb.velocity = dir * controller.status.CurrentMoveSpeed * Time.fixedDeltaTime * 10;
    }

    public virtual void CheckAttackCooltime()
    {
        //���� ��Ÿ���� ���� �� ��Ÿ�� ����
        if (controller.status.CheckAttackCooltime > 0)
        {
            controller.status.CheckAttackCooltime -= Time.deltaTime;
            if (controller.status.CheckAttackCooltime <= 0)
                controller.status.CheckAttackCooltime = 0;
        }
    }

    //���� �������� üũ
    public virtual bool CheckAttack()
    {
        //���� ó��
        if (controller.attackTarget == null || controller.attackTarget.currentState == EnemyState.Die)
            controller.FindAttackTarget();

        if (controller.attackTarget == null) return false;
        if (controller.status.CheckAttackCooltime > 0) return false;

        //���� ������ �Ÿ����� üũ �� �����ϸ� ���� ���·� ��ȯ
        if (Vector2.Distance(controller.attackTarget.transform.position, controller.transform.position) <= controller.status.CurrentAttackDistance)
        {
            controller.ChangeState(Define.RangerState.Attack);
            return true;
        }
        return false;
    }

    //���� ó��
    public virtual void Attack()
    {
        controller.routines.Add("attack", controller.StartCoroutine(AttackRoutine()));
    }

    //���� ó�� ��ƾ
    public virtual IEnumerator AttackRoutine()
    {
        controller.Stop();
        controller.status.CheckAttackCooltime = controller.status.CurrentAttackSpeed;
        Managers.Battle.AttackCalculation(controller, controller.attackTarget);
        yield return attackWaitForSeceonds; //�ִϸ��̼� �ð� ��ٸ��� ����
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

    //��ų ó�� ��ƾ
    public virtual IEnumerator SkillRoutine()
    {
        controller.Stop();
        Debug.Log("��ų ����");
        yield return skillWaitForSeconds; //�ִϸ��̼� �ð� ��ٸ��� ����
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