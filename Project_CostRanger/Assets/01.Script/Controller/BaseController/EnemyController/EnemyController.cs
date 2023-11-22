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
    public EnemyState setState;
    private EnemyState currentState;
    public Dictionary<EnemyState, State<EnemyController>> states;
    public StateMachine<EnemyController> stateMachine;
    public bool isDead;
    public bool isInit;

    //Enemy ETC
    public Rigidbody2D rb;
    public BaseController attackTarget;

    public void Init(Enemy _enemy, EnemyControllerData _data, EnemyStatus _status, Dictionary<EnemyState, State<EnemyController>> _states)
    {
        enemy = _enemy;
        data = _data;
        status = _status;
        states = _states;
        stateMachine = new StateMachine<EnemyController>(this, states[EnemyState.Stay]);

        rb = gameObject.GetOrAddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.isKinematic = true;

        routines = new Dictionary<string, Coroutine>();
        direction = Direction.Left;
        isDead = false;
        isInit = true;
    }

    public void ChangeState(EnemyState _nextState, bool _isChangeSameState = false)
    {
        if (!isInit) return;
        if (currentState == _nextState)
        {
            if (_isChangeSameState)
                stateMachine.ChangeState(states[_nextState]);
            return;
        }
        currentState = _nextState;
        setState = _nextState;
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
        if (currentState != setState)
            ChangeState(setState);
    }

    public override void Hit(float _damage)
    {

    }

    public override void GetDamage(float _damage)
    {

    }

    public void Follow()
    {
        enemy.Follow();
    }

    public void SetAttackTarget(BaseController _attackTarget)
    {
        attackTarget = _attackTarget;
    }

    public override void Die()
    {
        StopAllCoroutines();
    }

    public override void CheckDie()
    {
        if (status.CurrentHP <= 0)
        {
            status.CurrentHP = 0;
            ChangeState(EnemyState.Die);
        }
    }
}
public class EnemyStatus : ControllerStatus
{
    public EnemyStatus(BaseController _controller,EnemyControllerData _data)
    {
        controller = _controller;

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
