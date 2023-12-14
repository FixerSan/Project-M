using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Ranger 
{
    protected RangerController controller;
    public List<Define.SpecialtyType> specialties;
    protected WaitForSeconds attackBeforeWaitForSeceonds;
    protected WaitForSeconds attackAfterWaitForSeceonds;
    protected WaitForSeconds skillBeforeWaitForSeconds;
    protected WaitForSeconds skillAfterWaitForSeconds;

    public virtual void AddAnimationHash()
    {
        controller.animationHash.Add(Define.RangerState.Idle, Animator.StringToHash("0_idle"));
        controller.animationHash.Add(Define.RangerState.Follow, Animator.StringToHash("1_Run"));
        controller.animationHash.Add(Define.RangerState.Attack, Animator.StringToHash("2_Attack_Normal"));
        controller.animationHash.Add(Define.RangerState.Die, Animator.StringToHash("4_Death"));
        controller.animationHash.Add(Define.RangerState.SkillCast, Animator.StringToHash("5_Skill_Normal"));
    }

    public virtual bool CheckFollow()
    {
        if (controller.attackTarget == null || controller.attackTarget.currentState == Define.EnemyState.Die)
            controller.FindAttackTarget();

        if (controller.attackTarget == null)
            return false;

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
        if (controller.attackTarget == null || controller.attackTarget.currentState == Define.EnemyState.Die)
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
        
        yield return attackBeforeWaitForSeceonds; //애니메이션 시간 기다리는 거임
        Managers.Battle.AttackCalculation(controller, controller.attackTarget);
        yield return attackAfterWaitForSeceonds; //애니메이션 시간 기다리는 거임

        controller.status.CheckAttackCooltime = controller.status.CurrentAttackSpeed;
        controller.ChangeState(Define.RangerState.Idle);
        controller.routines.Remove("attack");
    }

    public virtual void CheckSkillCooltime()
    {
        if (controller.isSkillUsing) return;
        if (controller.status.CheckSkillCooltime > 0)
        {
            controller.status.CheckSkillCooltime -= Time.deltaTime;
            if (controller.status.CheckSkillCooltime <= 0)
                controller.status.CheckSkillCooltime = 0;
        }
    }

    public virtual void UseSkill()
    {
        if (controller.status.CheckSkillCooltime <= 0)
        {
            controller.ChangeState(Define.RangerState.SkillCast);
        }
    }

    public virtual bool CheckCanUseSkill()
    {
        if (!Managers.Game.battleStageSystem.isCanUseSkill) return false;
        if (!Managers.Game.battleStageSystem.isAutoSkill) return false;
        if (controller.status.CheckSkillCooltime == 0)
        {
            controller.ChangeState(Define.RangerState.SkillCast);
            return true;
        }

        return false;
    }

    public virtual void Skill()
    {
        if (controller.routines.TryGetValue("attack", out Coroutine _routine))
        {
            controller.StopCoroutine(_routine);
            controller.routines.Remove("attack");
        }
        controller.routines.Add("skill", controller.StartCoroutine(SkillRoutine()));
    }

    //스킬 처리 루틴
    public virtual IEnumerator SkillRoutine()
    {
        Managers.Game.battleStageSystem.UseRangerSkill(controller.data.UID);
        controller.Stop();
        Debug.Log("스킬 사용됨");
        yield return skillBeforeWaitForSeconds; //애니메이션 시간 기다리는 거임
        yield return skillAfterWaitForSeconds; //애니메이션 시간 기다리는 거임
        controller.ChangeState(Define.RangerState.Idle);
        controller.status.CheckSkillCooltime = controller.status.CurrentSkillCooltime;
        controller.routines.Remove("skill");
    }

    public virtual void GetDamage(float _damage)
    {
        controller.status.CurrentHP -= _damage;
    }

    public virtual void Hit(float _damage)
    {
        GetDamage(_damage);
    }
}

namespace Rangers
{
    public class Base : Ranger
    {
        public Base(RangerController _controller)
        {
            controller = _controller;
            attackBeforeWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackBeforeTime);
            attackAfterWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackAfterTime);

            skillBeforeWaitForSeconds = new WaitForSeconds(Define.normalSkillBeforeTime);
            skillAfterWaitForSeconds = new WaitForSeconds(Define.normalSkillAfterTime);
            specialties = new List<Define.SpecialtyType>();
        }
    }

    public class UnemployedKnight : Ranger
    {
        public UnemployedKnight(RangerController _controller)
        {
            controller = _controller;
            attackBeforeWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackBeforeTime);
            attackAfterWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackAfterTime);

            skillBeforeWaitForSeconds = new WaitForSeconds(Define.normalSkillBeforeTime);
            skillAfterWaitForSeconds = new WaitForSeconds(Define.normalSkillAfterTime);
            specialties = new List<Define.SpecialtyType>();
        }

        //스킬 처리 루틴
        public override IEnumerator SkillRoutine()
        {
            Managers.Game.battleStageSystem.UseRangerSkill(controller.data.UID);
            controller.Stop();
            Debug.Log("스킬 사용됨");
            yield return skillBeforeWaitForSeconds; //애니메이션 시간 기다리는 거임
            Managers.Battle.AttackCalculation(controller, controller.attackTarget, controller.status.CurrentAttackForce * 2);
            yield return skillAfterWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.ChangeState(Define.RangerState.Idle);
            controller.status.CheckSkillCooltime = controller.status.CurrentSkillCooltime;
            controller.routines.Remove("skill");
        }
    }

    public class BoringSpearman : Ranger
    {
        public BoringSpearman(RangerController _controller)
        {
            controller = _controller;
            attackBeforeWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackBeforeTime);
            attackAfterWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackAfterTime);

            skillBeforeWaitForSeconds = new WaitForSeconds(Define.normalSkillBeforeTime);
            skillAfterWaitForSeconds = new WaitForSeconds(Define.normalSkillAfterTime);
            specialties = new List<Define.SpecialtyType>();
        }

        //스킬 처리 루틴
        public override IEnumerator SkillRoutine()
        {
            Managers.Game.battleStageSystem.UseRangerSkill(controller.data.UID);
            controller.Stop();
            Debug.Log("스킬 사용됨");
            yield return skillBeforeWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.AddBuffAndNerf(new BuffsAndNerfs.BoringSpearmanSkill(controller, 4.3f));
            yield return skillAfterWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.ChangeState(Define.RangerState.Idle);
            controller.status.CheckSkillCooltime = controller.status.CurrentSkillCooltime;
            controller.routines.Remove("skill");
        }
    }
    public class DullAxeman : Ranger
    {
        public DullAxeman(RangerController _controller)
        {
            controller = _controller;
            attackBeforeWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackBeforeTime);
            attackAfterWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackAfterTime);

            skillBeforeWaitForSeconds = new WaitForSeconds(Define.normalSkillBeforeTime);
            skillAfterWaitForSeconds = new WaitForSeconds(Define.normalSkillAfterTime);
            specialties = new List<Define.SpecialtyType>();
        }

        //스킬 처리 루틴
        public override IEnumerator SkillRoutine()
        {
            Managers.Game.battleStageSystem.UseRangerSkill(controller.data.UID);
            controller.Stop();
            Debug.Log("스킬 사용됨");
            yield return skillBeforeWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.AddBuffAndNerf(new BuffsAndNerfs.DullAxemanSkill(controller, 10f));
            yield return skillAfterWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.ChangeState(Define.RangerState.Idle);
            controller.status.CheckSkillCooltime = controller.status.CurrentSkillCooltime;
            controller.routines.Remove("skill");
        }
    }

    public class StrangeAssassin : Ranger
    {
        public StrangeAssassin(RangerController _controller)
        {
            controller = _controller;
            attackBeforeWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackBeforeTime);
            attackAfterWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackAfterTime);

            skillBeforeWaitForSeconds = new WaitForSeconds(Define.normalSkillBeforeTime);
            skillAfterWaitForSeconds = new WaitForSeconds(Define.normalSkillAfterTime);
            specialties = new List<Define.SpecialtyType>();
        }

        //스킬 처리 루틴
        public override IEnumerator SkillRoutine()
        {
            Managers.Game.battleStageSystem.UseRangerSkill(controller.data.UID);
            controller.Stop();
            Debug.Log("스킬 사용됨");
            yield return skillBeforeWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.AddBuffAndNerf(new BuffsAndNerfs.StrangeAssassinSkill(controller, 4f));
            yield return skillAfterWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.ChangeState(Define.RangerState.Idle);
            controller.status.CheckSkillCooltime = controller.status.CurrentSkillCooltime;
            controller.routines.Remove("skill");
        }
    }

    public class GoofyHammeman : Ranger
    {
        public GoofyHammeman(RangerController _controller)
        {
            controller = _controller;
            attackBeforeWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackBeforeTime);
            attackAfterWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackAfterTime);

            skillBeforeWaitForSeconds = new WaitForSeconds(Define.normalSkillBeforeTime);
            skillAfterWaitForSeconds = new WaitForSeconds(Define.normalSkillAfterTime);
            specialties = new List<Define.SpecialtyType>();
        }

        //스킬 처리 루틴
        public override IEnumerator SkillRoutine()
        {
            Managers.Game.battleStageSystem.UseRangerSkill(controller.data.UID);
            controller.Stop();
            Debug.Log("스킬 사용됨");
            yield return skillBeforeWaitForSeconds; //애니메이션 시간 기다리는 거임
            //광역 공격 스킬
            AreaAttack attack = Managers.Resource.Instantiate("GoofyHammemanSkill", controller.attackTrans).GetComponent<AreaAttack>();
            attack.Attack(controller, Define.BattleEntityType.Ranger, controller.status.CurrentAttackForce * 1.6f);
            Managers.Resource.Destroy(attack.gameObject);
            yield return skillAfterWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.ChangeState(Define.RangerState.Idle);
            controller.status.CheckSkillCooltime = controller.status.CurrentSkillCooltime;
            controller.routines.Remove("skill");
        }
    }

    public class ScaredThug : Ranger
    {
        public ScaredThug(RangerController _controller)
        {
            controller = _controller;
            attackBeforeWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackBeforeTime);
            attackAfterWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackAfterTime);
            skillBeforeWaitForSeconds = new WaitForSeconds(Define.normalSkillBeforeTime);
            skillAfterWaitForSeconds = new WaitForSeconds(Define.normalSkillAfterTime);
            specialties = new List<Define.SpecialtyType>();
        }

        //스킬 처리 루틴
        public override IEnumerator SkillRoutine()
        {
            Managers.Game.battleStageSystem.UseRangerSkill(controller.data.UID);
            controller.Stop();
            Debug.Log("스킬 사용됨");
            yield return skillBeforeWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.AddBuffAndNerf(new BuffsAndNerfs.ScaredThugSkill(controller, 5f));
            yield return skillAfterWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.ChangeState(Define.RangerState.Idle);
            controller.status.CheckSkillCooltime = controller.status.CurrentSkillCooltime;
            controller.routines.Remove("skill");
        }

        public override void GetDamage(float _damage)
        {
            if(controller.isSkillUsing)
                controller.status.CurrentHP -= 0;

            controller.status.CurrentHP -= _damage;
        }
    }
    
    public class ClumsyArcher : Ranger
    {
        public ClumsyArcher(RangerController _controller)
        {
            controller = _controller;
            attackBeforeWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackBeforeTime);
            attackAfterWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackAfterTime);
            skillBeforeWaitForSeconds = new WaitForSeconds(Define.normalSkillBeforeTime);
            skillAfterWaitForSeconds = new WaitForSeconds(Define.normalSkillAfterTime);
            specialties = new List<Define.SpecialtyType>();
        }

        //스킬 처리 루틴
        public override IEnumerator SkillRoutine()
        {
            Managers.Game.battleStageSystem.UseRangerSkill(controller.data.UID);
            controller.Stop();
            Debug.Log("스킬 사용됨");
            yield return skillBeforeWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.status.CurrentHP += controller.status.CurrentMaxHP * 0.7f;
            yield return skillAfterWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.ChangeState(Define.RangerState.Idle);
            controller.status.CheckSkillCooltime = controller.status.CurrentSkillCooltime;
            controller.routines.Remove("skill");
        }

        public override void GetDamage(float _damage)
        {
            if(controller.isSkillUsing)
                controller.status.CurrentHP -= 0;

            controller.status.CurrentHP -= _damage;
        }

        public override IEnumerator AttackRoutine()
        {
            controller.Stop();
            controller.status.CheckAttackCooltime = controller.status.CurrentAttackSpeed;

            yield return attackBeforeWaitForSeceonds; //애니메이션 시간 기다리는 거임
            LongAttack attack = Managers.Resource.Instantiate("ArcherAttack", controller.attackTrans).GetComponent<LongAttack>();
            attack.Attack(controller, controller.attackTarget);
            yield return attackAfterWaitForSeceonds; //애니메이션 시간 기다리는 거임

            controller.status.CheckAttackCooltime = controller.status.CurrentAttackSpeed;
            controller.ChangeState(Define.RangerState.Idle);
            controller.routines.Remove("attack");
        }
    }

    public class LenientNinja : Ranger
    {
        public LenientNinja(RangerController _controller)
        {
            controller = _controller;
            attackBeforeWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackBeforeTime);
            attackAfterWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackAfterTime);
            skillBeforeWaitForSeconds = new WaitForSeconds(Define.normalSkillBeforeTime);
            skillAfterWaitForSeconds = new WaitForSeconds(Define.normalSkillAfterTime);
            specialties = new List<Define.SpecialtyType>();
        }

        //스킬 처리 루틴
        public override IEnumerator SkillRoutine()
        {
            Managers.Game.battleStageSystem.UseRangerSkill(controller.data.UID);
            controller.Stop();
            Debug.Log("스킬 사용됨");
            yield return skillBeforeWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.AddBuffAndNerf(new BuffsAndNerfs.LenientNinjaSkill(controller, 7));
            yield return skillAfterWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.ChangeState(Define.RangerState.Idle);
            controller.status.CheckSkillCooltime = controller.status.CurrentSkillCooltime;
            controller.routines.Remove("skill");
        }

        public override void GetDamage(float _damage)
        {
            if (controller.isSkillUsing)
                controller.status.CurrentHP -= 0;

            controller.status.CurrentHP -= _damage;
        }

        public override IEnumerator AttackRoutine()
        {
            controller.Stop();
            controller.status.CheckAttackCooltime = controller.status.CurrentAttackSpeed;

            yield return attackBeforeWaitForSeceonds; //애니메이션 시간 기다리는 거임
            LongAttack attack = Managers.Resource.Instantiate("NinjaAttack", controller.attackTrans).GetComponent<LongAttack>();
            attack.Attack(controller, controller.attackTarget);
            yield return attackAfterWaitForSeceonds; //애니메이션 시간 기다리는 거임

            controller.status.CheckAttackCooltime = controller.status.CurrentAttackSpeed;
            controller.ChangeState(Define.RangerState.Idle);
            controller.routines.Remove("attack");
        }
    }
    public class FieryArcher : Ranger
    {
        public FieryArcher(RangerController _controller)
        {
            controller = _controller;
            attackBeforeWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackBeforeTime);
            attackAfterWaitForSeceonds = new WaitForSeconds(Define.magicAndNormalAttackAfterTime);
            skillBeforeWaitForSeconds = new WaitForSeconds(Define.normalSkillBeforeTime);
            skillAfterWaitForSeconds = new WaitForSeconds(Define.normalSkillAfterTime);
            specialties = new List<Define.SpecialtyType>();
        }

        //스킬 처리 루틴
        public override IEnumerator SkillRoutine()
        {
            Managers.Game.battleStageSystem.UseRangerSkill(controller.data.UID);
            controller.Stop();
            Debug.Log("스킬 사용됨");
            yield return skillBeforeWaitForSeconds; //애니메이션 시간 기다리는 거임
            for (int i = 0; i < Managers.Object.Enemies.Count; i++)
            {
                LongAttack attack = Managers.Resource.Instantiate("ArcherAttack", controller.attackTrans).GetComponent<LongAttack>();
                attack.Attack(controller, Managers.Object.Enemies[i], controller.status.CurrentAttackForce * 2);
            }
            yield return skillAfterWaitForSeconds; //애니메이션 시간 기다리는 거임
            controller.ChangeState(Define.RangerState.Idle);
            controller.status.CheckSkillCooltime = controller.status.CurrentSkillCooltime;
            controller.routines.Remove("skill");
        }

        public override void GetDamage(float _damage)
        {
            if (controller.isSkillUsing)
                controller.status.CurrentHP -= 0;

            controller.status.CurrentHP -= _damage;
        }

        public override IEnumerator AttackRoutine()
        {
            controller.Stop();
            controller.status.CheckAttackCooltime = controller.status.CurrentAttackSpeed;

            yield return attackBeforeWaitForSeceonds; //애니메이션 시간 기다리는 거임
            LongAttack attack = Managers.Resource.Instantiate("ArcherAttack", controller.attackTrans).GetComponent<LongAttack>();
            attack.Attack(controller, controller.attackTarget);
            yield return attackAfterWaitForSeceonds; //애니메이션 시간 기다리는 거임

            controller.status.CheckAttackCooltime = controller.status.CurrentAttackSpeed;
            controller.ChangeState(Define.RangerState.Idle);
            controller.routines.Remove("attack");
        }
    }
}