using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EnemyController : BaseController
{
    //Enemy Identity
    public Enemy enemy;
    public EnemyControllerData data;

    //Enemy Stage
    public EnemyState State;
    private EnemyState state;
    public Dictionary<EnemyState, State<EnemyController>> states;
    public StateMachine<EnemyController> stateMachine;
    public bool isDead;
    public bool isInit;

    //Enemy ETC
    public Rigidbody2D rb;
    public Dictionary<string, Coroutine> routines;

    public void Init(Enemy _enemy, EnemyControllerData _data, EnemyStatus _status, Dictionary<EnemyState, State<EnemyController>> _states)
    {
        enemy = _enemy;
        data = _data;
        status = _status;
        states = _states;
        stateMachine = new StateMachine<EnemyController>(this, states[EnemyState.Idle]);

        rb = gameObject.GetOrAddComponent<Rigidbody2D>();
        routines = new Dictionary<string, Coroutine>();

        isDead = false;
        isInit = true;
    }

    public void ChangeState(EnemyState _nextState, bool _isChangeSameState = false)
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
public class EnemyStatus : ControllerStatus
{
    public EnemyStatus(EnemyControllerData _data)
    {
        //���ݷ�
        defaultAttackForce = _data.attackForce;
        currentAttackForce = _data.attackForce;

        //���ݼӵ�
        defaultAttackSpeed = _data.attackSpeed;
        currentAttackSpeed = _data.attackSpeed;

        //���� ��Ÿ�
        defaultAttackDistance = _data.attackDistance;
        currentAttackDistance = _data.attackDistance;

        //ũ��Ƽ�� ����
        defaultCriticalForce = _data.criticalForce;
        currentCriticalForce = _data.criticalForce;

        //ũ������ Ȯ��
        defaultCriticalProbability = _data.criticalProbability;
        currentCriticalProbability = _data.criticalProbability;

        //����
        defaultDefenseForce = _data.defenseForce;
        CurrentDefenseForce = _data.defenseForce;

        //ü��
        defaultHP = _data.hp;
        currentHP = _data.hp;
        curretnMaxHP = _data.hp;

        //�̵� �ӵ�
        defaultMoveSpeed = _data.moveSpeed;
        currentMoveSpeed = _data.moveSpeed;

        //��ų ��Ÿ��
        defaultSkillCooltime = _data.skillCooltime;
        currentSkillCooltime = _data.skillCooltime;
    }
}
