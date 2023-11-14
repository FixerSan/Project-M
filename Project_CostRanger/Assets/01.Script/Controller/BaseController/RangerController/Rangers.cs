using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ranger 
{
    private RangerController controller;
    public Define.Specialty[] specialty;
    protected WaitForSeconds attackWaitForSeceonds;
    protected WaitForSeconds skillWaitForSeconds;


    //����
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

    //���� �������� üũ
    public virtual bool CheckAttack()
    {
        //���� ��Ÿ���� ���� �� ��Ÿ�� ����
        if (controller.status.CheckAttackCooltime > 0)
        {
            controller.status.CheckAttackCooltime -= Time.deltaTime;
            if (controller.status.CheckAttackCooltime <= 0)
                controller.status.CheckAttackCooltime = 0;
        }

        //���� ó��
        if (controller.attackTarget == null) return false;
        if (controller.status.CheckAttackCooltime > 0) return false;
        if (controller.currentState != Define.RangerState.Follow) return false;

        //���� ������ �Ÿ����� üũ �� �����ϸ� ���� ���·� ��ȯ
        if (Vector2.Distance(controller.attackTarget.transform.position, controller.transform.position) < controller.status.CurrentAttackDistance)
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
        controller.status.CheckAttackCooltime = controller.status.CurrentAttackSpeed;
        Managers.Battle.AttackCalculation(controller, controller.attackTarget, (_damage) => { /*controller.mvpPoint += _damage;*/ });
        //Managers.Game.battleInfo.UpdateMVPPoints();
        yield return attackWaitForSeceonds; //�ִϸ��̼� �ð� ��ٸ��� ����
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