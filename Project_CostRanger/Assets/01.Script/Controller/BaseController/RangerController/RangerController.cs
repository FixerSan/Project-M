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
    public RangerState state;
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
        stateMachine.ChangeState(states[_nextState]);
    }

    private void Update()
    {
        if (isInit) return;
        stateMachine.UpdateState();
    }

    public override void Hit(float _damage)
    {

    }

    public override void GetDamage(float _damage)
    {

    }
}

public abstract class ControllerStatus
{
    //공격력
    public float defaultAttackForce;
    protected float currentAttackForce;
    public float CurrentAttackForce 
    {
        get
        {
            return currentAttackForce;
        }

        set
        {
            currentAttackForce = value;
        }
    }

    //공격속도
    public float defaultAttackSpeed;
    protected float currentAttackSpeed;
    public float CurrentAttackSpeed
    {
        get
        {
            return currentAttackSpeed;
        }

        set
        {
            currentAttackSpeed = value;
        }
    }

    //공격 사거리
    public float defaultAttackDistance;
    protected float currentAttackDistance;
    public float CurrentAttackDistance
    {
        get
        {
            return currentAttackDistance;
        }

        set
        {
            currentAttackDistance = value;
        }
    }

    //치명타 세기
    public float defaultCriticalForce;
    protected float currentCriticalForce;
    public float CurrentCriticalForce
    {
        get
        {
            return currentCriticalForce;
        }

        set
        {
            currentCriticalForce = value;
        }
    }

    //치명타 확률
    public float defaultCriticalProbability;
    protected float currentCriticalProbability;
    public float CurrentCriticalProbability
    {
        get
        {
            return currentCriticalProbability;
        }

        set
        {
            currentCriticalProbability = value;
        }
    }

    //방어력
    public float defaultDefenseForce;
    protected float currentDefenseForce;
    public float CurrentDefenseForce
    {
        get
        {
            return currentDefenseForce;
        }

        set
        {
            currentDefenseForce = value;
        }
    }

    //체력
    public float defaultHP;
    protected float curretnMaxHP; 
    public float CurrentMaxHP
    {
        get
        {
            return curretnMaxHP;
        }

        set 
        {
            curretnMaxHP = value;
        }
    }
    protected float currentHP;
    public float CurrentHP
    {
        get
        {
            return currentHP;
        }

        set
        {
            currentHP = value;
        }
    }

    //이동속도
    public float defaultMoveSpeed;
    protected float currentMoveSpeed;
    public float CurrentMoveSpeed
    {
        get
        {
            return currentMoveSpeed;
        }

        set
        {
            currentMoveSpeed = value;
        }
    }

    //스킬 쿨타임
    public float defaultSkillCooltime;
    protected float currentSkillCooltime;
    public float CurrentSkillCooltime
    {
        get
        {
            return currentSkillCooltime;
        }

        set
        {
            currentSkillCooltime = value;
        }
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
