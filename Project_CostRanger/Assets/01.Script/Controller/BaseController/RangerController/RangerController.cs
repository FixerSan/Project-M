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
    public RangerState setState;   //�ܺο��� ������Ʈ�� �����ϱ� ���� ���� ������Ƽ ���� ����
    public RangerState currentState;
    public Dictionary<RangerState, State<RangerController>> states;
    public StateMachine<RangerController> stateMachine;
    public bool isDead;
    public bool isInit;

    //Ranger ETC
    public Rigidbody2D rb;
    public Dictionary<string, Coroutine> routines;
    public EnemyController attackTarget;

    public void Init(Ranger _ranger, RangerControllerData _data, RangerStatus _status, Dictionary<RangerState, State<RangerController>> _states)
    {
        ranger = _ranger;
        data = _data;
        status = _status;
        states = _states;
        stateMachine = new StateMachine<RangerController>(this, states[RangerState.Idle]);

        rb = gameObject.GetOrAddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.isKinematic = true;

        routines = new Dictionary<string, Coroutine>();

        isDead = false;
        isInit = true;

    }

    public void ChangeState(RangerState _nextState, bool _isChangeSameState = false)
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

    public void SetAttackTarget(EnemyController _attackTarget)
    {
        attackTarget = _attackTarget;
    }
}

public class RangerStatus : ControllerStatus
{
    public RangerStatus(RangerControllerData _data)
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

        //�� ��� �ð�
        checkAttackCooltime = 0;
        checkSkillCooltime = _data.skillCooltime;
    }
}
