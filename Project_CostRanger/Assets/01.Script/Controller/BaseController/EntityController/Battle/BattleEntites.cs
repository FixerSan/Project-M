using BattleEntityStates.Base;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BattleEntity : IAttackable, IHittable
{
    //컨트롤러
    protected BattleEntityController controller;
    //데이터
    public BattleEntityData data;

    //공격 가능한지 체크
    public virtual bool CheckAttack()
    {
        //공격 쿨타임이 있을 때 쿨타임 감소
        if (controller.battleEntityStatus.checkAttackTime > 0)
        {
            controller.battleEntityStatus.checkAttackTime -= Time.deltaTime;
            if (controller.battleEntityStatus.checkAttackTime <= 0)
                controller.battleEntityStatus.checkAttackTime = 0;
        }

        //예외 처리
        if (controller.attackTarget == null) return false;
        if (controller.battleEntityStatus.checkAttackTime > 0) return false;
        if (controller.state != Define.BattleEntityState.Follow) return false;

        //공격 가능한 거리인지 체크 후 가능하면 공격 상태로 변환
        if (Vector2.Distance(controller.attackTarget.transform.position, controller.transform.position) < data.canAttackDistance)
        {
            controller.ChangeState(Define.BattleEntityState.Attack);
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
        controller.battleEntityStatus.checkAttackTime = controller.battleEntityStatus.currentAttackCycle;
        Managers.Battle.AttackCalculation(controller, controller.attackTarget, (_damage)=> { /*controller.mvpPoint += _damage;*/ });
        Managers.Game.battleInfo.UpdateMVPPoints();
        yield return new WaitForSeconds(1);
        controller.routines.Remove("attack");
        controller.ChangeState(Define.BattleEntityState.Follow);
    }

    //피격 처리
    public virtual void Hit(int _damage)
    {
        controller.GetDamage(_damage);
    }

    //데미지 처리
    public virtual void GetDamage(int _damage)
    {
        if(controller.battleEntityStatus.buff.CheckCanMiss())
        {
            Managers.UI.MakeWorldText("Miss", controller.transform.position + controller.textOffset, Define.TextType.Damage);
            return;
        }
        controller.battleEntityStatus.CurrentHP -= _damage;
        Managers.UI.MakeWorldText(_damage.ToString(), controller.transform.position + controller.textOffset, Define.TextType.Damage);
        if(controller.battleEntityStatus.CurrentHP <= 0)
        {
            controller.battleEntityStatus.CurrentHP = 0;
            controller.ChangeState(Define.BattleEntityState.Die);
        }
    }

    //이동 처리
    public virtual void Move()
    {
        if(controller.moveTarget != null)
        {
            Vector2 dir = (controller.moveTarget.position - controller.transform.position).normalized;
            controller.rb.velocity = (dir * controller.battleEntityStatus.moveSpeed) * Time.fixedDeltaTime * 20;
        }
    }

    //추격 처리
    public virtual void Follow()
    {
        if (controller.attackTarget != null)
        {
            if (Vector2.Distance(controller.attackTarget.transform.position, controller.transform.position) < data.canAttackDistance)
            {
                controller.Stop();
                return;
            }
            Vector2 dir = (controller.attackTarget.transform.position - controller.transform.position).normalized;
            controller.rb.velocity = (dir * controller.battleEntityStatus.moveSpeed) * Time.fixedDeltaTime * 20;
        }
    }

    //정지 처리
    public virtual bool CheckStop()
    {
        if (Vector2.Distance(controller.moveTarget.position, controller.transform.position) < 0.1f)
        {
            controller.ChangeState(Define.BattleEntityState.Idle);
            return true;
        }
        return false;
    }

    //스킬 사용이 가능한지 체크 후 가능하다면 사용
    public virtual bool CheckCanUseSkill()
    {
        //스킬 쿨타임이 있다면 쿨타임 감소
        if(controller.battleEntityStatus.currentSkillCooltime > 0)
        {
            controller.battleEntityStatus.currentSkillCooltime -= Time.deltaTime;
            if (controller.battleEntityStatus.currentSkillCooltime <= 0)
                controller.battleEntityStatus.currentSkillCooltime = 0;
        }
        //예외처리
        if (Managers.Screen.isSkillCasting) return false;
        if (controller.entityType == Define.BattleEntityType.Enemy) 
        {
            if (controller.state != Define.BattleEntityState.Follow) return false;
            if (controller.battleEntityStatus.currentSkillCooltime <= 0)
            {
                //스킬 사용 상태로 변경
                controller.ChangeState(Define.BattleEntityState.SkillCast);
                return true;
            }
            else return false;
        }
        //오토 스킬 처리
        if (!Managers.Game.battleInfo.isAutoSkill) return false;
        if (controller.state != Define.BattleEntityState.Follow) return false;
        if (controller.battleEntityStatus.currentSkillCooltime <= 0)
        {
            controller.ChangeState(Define.BattleEntityState.SkillCast);
            return true;
        }
        else return false;
    }

    public abstract void Skill();
    //스킬 공통 부분 처리
    public void BaseSkill(BattleEntityData _data)
    {
        controller.StopAllRoutine();
        Managers.Screen.SkillScreen(_data);
        controller.ChangeStateWithDelay(Define.BattleEntityState.Follow, 2);
    }
}

namespace BattleEntites
{
    public class Warrior : BattleEntity
    {
        public override void Skill()
        {
            BaseSkill(controller.entity.data);
            for (int i = 0; i < Managers.Object.Armys.Count; i++)
            {
                Managers.Object.Armys[i].SetBuff_PlusSpeed(3f,Managers.Object.Armys[i].battleEntityStatus.currentAttackCycle * 0.25f);
            }
        }

        public Warrior(BattleEntityController _controller, BattleEntityData _data)
        {
            controller = _controller;
            data = _data;
        }
    }

    public class Tank : BattleEntity
    {
        public override void Skill()
        {
            BaseSkill(controller.entity.data);
            for (int i = 0; i < Managers.Object.Armys.Count; i++)
            {
                Managers.Object.Armys[i].SetBuff_SetMissCount(3);
            }
        }

        public Tank(BattleEntityController _controller, BattleEntityData _data)
        {
            controller = _controller;
            data = _data;
        }

    }

    public class Wizard : BattleEntity
    {
        public override void Skill()
        {
            BaseSkill(controller.entity.data);
            for (int i = 0; i < Managers.Object.Armys.Count; i++)
            {
                Managers.Object.Armys[i].Heal((int)((Managers.Object.Armys[i].battleEntityStatus.maxHP - Managers.Object.Armys[i].battleEntityStatus.CurrentHP) * 0.25f));
            }
        }

        public Wizard(BattleEntityController _controller, BattleEntityData _data)
        {
            controller = _controller;
            data = _data;
        }
    }

    public class Three : BattleEntity
    {

        public override void Skill()
        {
            BaseSkill(controller.entity.data);
            Managers.Battle.AttackCalculation(controller.battleEntityStatus.attackForce * 3, controller.attackTarget, (_damage) => { controller.mvpPoint += _damage; });
        }

        public Three(BattleEntityController _controller, BattleEntityData _data)
        {
            controller = _controller;
            data = _data;
        }
    }

    public class Four : BattleEntity
    {

        public override void Skill()
        {
            BaseSkill(controller.entity.data);
            Managers.Battle.AttackCalculation(controller.battleEntityStatus.attackForce * 3, controller.attackTarget, (_damage) => { controller.mvpPoint += _damage; });
        }

        public Four (BattleEntityController _controller, BattleEntityData _data)
        {
            controller = _controller;
            data = _data;
        }
    }

    public class Five : BattleEntity
    {

        public override void Skill()
        {
            BaseSkill(controller.entity.data);
            Managers.Battle.AttackCalculation(controller.battleEntityStatus.attackForce * 3, controller.attackTarget, (_damage) => { controller.mvpPoint += _damage; });
        }

        public Five(BattleEntityController _controller, BattleEntityData _data)
        {
            controller = _controller;
            data = _data;
        }
    }
}
