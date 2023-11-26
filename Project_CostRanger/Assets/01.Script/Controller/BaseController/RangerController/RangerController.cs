using EnemyStates.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static Define;

public class RangerController : BaseController
{
    //Ranger Identity
    public Ranger ranger;
    public RangerControllerData data;

    //Ranger State
    public RangerState setState;   //외부에서 스테이트를 조작하기 위해 만든 프로퍼티 같은 느낌
    public RangerState currentState;
    public Dictionary<RangerState, State<RangerController>> states;
    public StateMachine<RangerController> stateMachine;
    public bool isDead;
    public bool isInit;

    //Ranger ETC
    public Rigidbody2D rb;
    public Animator animator;
    public Dictionary<RangerState, int> animationHash;
    public EnemyController attackTarget;

    public void Init(Ranger _ranger, RangerControllerData _data, RangerStatus _status, Dictionary<RangerState, State<RangerController>> _states)
    {
        ranger = _ranger;
        data = _data;
        status = _status;
        states = _states;
        stateMachine = new StateMachine<RangerController>(this, states[RangerState.Stay]);

        rb = gameObject.GetOrAddComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        animator = Util.FindChild<Animator>(gameObject, "UnitRoot");
        animationHash = new Dictionary<RangerState, int>();
        ranger.AddAnimationHash();

        routines = new Dictionary<string, Coroutine>();

        direction = Direction.Left;
        isDead = false;
        isInit = true;
    }

    public void ChangeState(RangerState _nextState, bool _isChangeSameState = false)
    {
        if (!isInit) return;
        int hash;
        if (currentState == _nextState)
        {
            if (_isChangeSameState)
            {
                stateMachine.ChangeState(states[_nextState]);
                if(animationHash.TryGetValue(_nextState, out hash))
                    animator.Play(hash);
            }
            return;
        }
        currentState = _nextState;
        setState = _nextState;
        stateMachine.ChangeState(states[_nextState]);
        if (animationHash.TryGetValue(_nextState, out hash))
            animator.Play(hash);
    }

    private void Update()
    {
        if (!isInit) return;
        stateMachine.UpdateState();
        CheckChangeState();
    }

    private void CheckChangeState()
    {
        if (currentState != setState)
            ChangeState(setState);
    }

    public override void Hit(float _damage)
    {
        GetDamage(_damage);
    }

    public override void GetDamage(float _damage)
    {
        status.CurrentHP -= _damage;
    }

    public override void Die()
    {
        StopAllCoroutines();
    }

    public void FindAttackTarget()
    {
        for (int i = 0; i < Managers.Object.Enemies.Count; i++)
        {
            if (attackTarget == null)
            {   
                attackTarget = Managers.Object.Enemies[i];
                continue;
            }

            if(Vector2.Distance(transform.position, attackTarget.transform.position) > Vector2.Distance(transform.position, Managers.Object.Enemies[i].transform.position))
                attackTarget = Managers.Object.Enemies[i];
        }
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
    }

    public override void CheckDie()
    {
        if (status.CurrentHP <= 0)
        {
            status.CurrentHP = 0;
            ChangeState(RangerState.Die);
        }
    }
}

public class RangerStatus : ControllerStatus
{
    public RangerStatus(BaseController _controller, RangerControllerData _data)
    {
        controller = _controller; 
        //공격력
        defaultAttackForce = _data.attackForce;
        currentAttackForce = _data.attackForce;

        //공격속도
        defaultAttackSpeed = _data.attackSpeed;
        currentAttackSpeed = _data.attackSpeed;
        
        //공격 사거리
        defaultAttackDistance = _data.attackDistance;
        currentAttackDistance = _data.attackDistance;

        //크리티컬 배율
        defaultCriticalForce = _data.criticalForce;
        currentCriticalForce = _data.criticalForce;

        //크리터컬 확률
        defaultCriticalProbability = _data.criticalProbability;
        currentCriticalProbability = _data.criticalProbability;

        //방어력
        defaultDefenseForce = _data.defenseForce;
        CurrentDefenseForce = _data.defenseForce;

        //체력
        defaultHP = _data.hp;
        currentHP = _data.hp;
        curretnMaxHP = _data.hp;

        //이동 속도
        defaultMoveSpeed = _data.moveSpeed;
        currentMoveSpeed = _data.moveSpeed;

        //스킬 쿨타임
        defaultSkillCooltime = _data.skillCooltime;
        currentSkillCooltime = _data.skillCooltime;

        //각 계산 시간
        checkAttackCooltime = 0;
        checkSkillCooltime = _data.skillCooltime;
    }
}
