using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class RangerController : BaseController
{
    //Ranger Identity
    public Ranger ranger;
    public RangerControllerData data;

    //Ranger State
    public RangerState State;   //외부에서 스테이트를 조작하기 위해 만든 프로퍼티 같은 느낌
    private RangerState state;
    public Dictionary<RangerState, State<RangerController>> states;
    public StateMachine<RangerController> stateMachine;
    public bool isDead;
    public bool isInit;

    //Ranger ETC
    public Rigidbody2D rb;
    public Dictionary<string, Coroutine> routines;

    public void Init(Ranger _ranger, RangerControllerData _data, RangerStatus _status, Dictionary<RangerState, State<RangerController>> _states)
    {
        ranger = _ranger;
        data = _data;
        status = _status;
        states = _states;
        stateMachine = new StateMachine<RangerController>(this, states[RangerState.Idle]);

        rb = gameObject.GetOrAddComponent<Rigidbody2D>();
        routines = new Dictionary<string, Coroutine>();

        isDead = false;
        isInit = true;
    }

    public void ChangeState(RangerState _nextState, bool _isChangeSameState = false)
    {
        if (!isInit) return;
        if (state == _nextState)
        {
            if (_isChangeSameState)
                stateMachine.ChangeState(states[_nextState]);
            return;
        }
        state = _nextState;
        State = _nextState;
        stateMachine.ChangeState(states[_nextState]);
    }

    private void Update()
    {
        if (!isInit) return;
        stateMachine.UpdateState();
        CheckChangeState();
    }

    private void CheckChangeState()
    {
        if (state != State)
            ChangeState(State);
    }

    public override void Hit(float _damage)
    {

    }

    public override void GetDamage(float _damage)
    {

    }
}

public class RangerStatus : ControllerStatus
{
    public RangerStatus(RangerControllerData _data)
    {
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
    }
}
