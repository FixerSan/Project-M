using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy
{
    protected EnemyController controller;
    public List<Define.SpecialtyType> specialties;

    protected WaitForSeconds attackBeforeWaitForSeceonds;
    protected WaitForSeconds attackAfterWaitForSeceonds;
    protected WaitForSeconds skillBeforeWaitForSeconds;
    protected WaitForSeconds skillAfterWaitForSeconds;

    protected Vector2 dir;


    public virtual void AddAnimationHash()
    {
        controller.animationHash.Add(Define.EnemyState.Idle, Animator.StringToHash("0_idle"));
        controller.animationHash.Add(Define.EnemyState.Follow, Animator.StringToHash("1_Run"));
        controller.animationHash.Add(Define.EnemyState.Attack, Animator.StringToHash("2_Attack_Normal"));
        controller.animationHash.Add(Define.EnemyState.Die, Animator.StringToHash("4_Death"));
        controller.animationHash.Add(Define.EnemyState.SkillCast, Animator.StringToHash("5_Skill_Normal"));
    }

    public virtual bool CheckFollow()
    {
        if (controller.attackTarget == null)
            controller.FindAttackTarget();

        if (Vector2.Distance(controller.attackTarget.transform.position, controller.transform.position) > controller.status.CurrentAttackDistance)
        {
            controller.ChangeState(Define.EnemyState.Follow);
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

    public virtual bool CheckStop()
    {
        if (controller.attackTarget == null)
        {
            controller.ChangeState(Define.EnemyState.Idle);
            return true;
        }

        if (Vector2.Distance(controller.attackTarget.transform.position, controller.transform.position) <= controller.status.CurrentAttackDistance)
        {
            controller.ChangeState(Define.EnemyState.Idle);
            return true;
        }

        return false;
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
        if (controller.attackTarget == null || controller.attackTarget.currentState == Define.RangerState.Die)
            controller.FindAttackTarget();

        if (controller.attackTarget == null) return false;
        if (controller.status.CheckAttackCooltime > 0) return false;

        //���� ������ �Ÿ����� üũ �� �����ϸ� ���� ���·� ��ȯ
        if (Vector2.Distance(controller.attackTarget.transform.position, controller.transform.position) <= controller.status.CurrentAttackDistance)
        {
            controller.ChangeState(Define.EnemyState.Attack);
            return true;
        }
        return false;
    }

    //���� ó��
    public virtual void Attack()
    {
        if (controller.routines.TryGetValue("attack", out Coroutine _routine))
        {
            controller.StopCoroutine(_routine);
            controller.routines.Remove("attack");
        }
        controller.routines.Add("attack", controller.StartCoroutine(AttackRoutine()));
    }

    //���� ó�� ��ƾ
    public virtual IEnumerator AttackRoutine()
    {
        controller.Stop();
        controller.status.CheckAttackCooltime = controller.status.CurrentAttackSpeed;
        //Managers.Battle.AttackCalculation(controller, controller.attackTarget, (_damage) => { /*controller.mvpPoint += _damage;*/ });
        //Managers.Game.battleInfo.UpdateMVPPoints();
        yield return attackBeforeWaitForSeceonds; //�ִϸ��̼� �ð� ��ٸ��� ����
        Managers.Battle.AttackCalculation(controller, controller.attackTarget);
        yield return attackAfterWaitForSeceonds; //�ִϸ��̼� �ð� ��ٸ��� ����
        controller.status.CheckAttackCooltime = controller.status.CurrentAttackSpeed;
        controller.ChangeState(Define.EnemyState.Idle);
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
            controller.ChangeState(Define.EnemyState.SkillCast);
        }
    }

    public virtual bool CheckCanUseSkill()
    {
        if (controller.status.CheckSkillCooltime == 0 && Vector2.Distance(controller.attackTarget.transform.position, controller.transform.position) <= controller.status.CurrentAttackDistance)
        {
            controller.ChangeState(Define.EnemyState.SkillCast);
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
        yield return skillBeforeWaitForSeconds; //�ִϸ��̼� �ð� ��ٸ��� ����
        Managers.Battle.AttackCalculation(controller, controller.attackTarget, controller.status.CurrentAttackForce * 1.5f);
        yield return skillAfterWaitForSeconds; //�ִϸ��̼� �ð� ��ٸ��� ����
        controller.ChangeState(Define.EnemyState.Idle);
        controller.status.CheckSkillCooltime = controller.status.CurrentSkillCooltime;
        controller.routines.Remove("skill");
    }
}

namespace Enemies
{
    public class Base : Enemy
    {
        public Base(EnemyController _controller)
        {
            controller = _controller;
            attackBeforeWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackBeforeTime);
            attackAfterWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackAfterTime);
            skillBeforeWaitForSeconds = new WaitForSeconds(Define.normalSkillBeforeTime);
            skillAfterWaitForSeconds = new WaitForSeconds(Define.normalSkillAfterTime);
            specialties = new List<Define.SpecialtyType>();
        }
    }

    public class WitchZombie : Enemy
    {
        public WitchZombie(EnemyController _controller)
        {
            controller = _controller;
            attackBeforeWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackBeforeTime);
            attackAfterWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackAfterTime);
            skillBeforeWaitForSeconds = new WaitForSeconds(Define.magicSkillBeforeTime);
            skillAfterWaitForSeconds = new WaitForSeconds(Define.magicSkillAfterTime);
            specialties = new List<Define.SpecialtyType>();
        }

        public override void AddAnimationHash()
        {
            controller.animationHash.Add(Define.EnemyState.Idle, Animator.StringToHash("0_idle"));
            controller.animationHash.Add(Define.EnemyState.Follow, Animator.StringToHash("1_Run"));
            controller.animationHash.Add(Define.EnemyState.Attack, Animator.StringToHash("2_Attack_Magic"));
            controller.animationHash.Add(Define.EnemyState.Die, Animator.StringToHash("4_Death"));
            controller.animationHash.Add(Define.EnemyState.SkillCast, Animator.StringToHash("5_Skill_Magic"));
        }
    }
}
