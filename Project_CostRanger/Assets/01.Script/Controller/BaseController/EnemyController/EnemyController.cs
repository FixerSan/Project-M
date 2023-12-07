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
    public EnemyState currentState;
    public Dictionary<EnemyState, State<EnemyController>> states;
    public StateMachine<EnemyController> stateMachine;
    public bool isDead;
    public bool isInit;

    //Enemy ETC
    public Rigidbody2D rb;
    public Animator animator;
    public Dictionary<EnemyState, int> animationHash;
    public RangerController attackTarget;

    public void Init(Enemy _enemy, EnemyControllerData _data, EnemyStatus _status, Dictionary<EnemyState, State<EnemyController>> _states)
    {
        enemy = _enemy;
        data = _data;
        status = _status;
        states = _states;
        stateMachine = new StateMachine<EnemyController>(this, states[EnemyState.Stay]);

        rb = gameObject.GetOrAddComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        animator = Util.FindChild<Animator>(gameObject, _recursive:true);
        animationHash = new Dictionary<EnemyState, int>();
        enemy.AddAnimationHash();


        routines = new Dictionary<string, Coroutine>();

        direction = Direction.Left;
        isDead = false;
        isInit = true;
        worldTextTrans = Util.FindChild<Transform>(gameObject, "Trans_WorldTest");

        SetHPBar();
    }

    public void ChangeState(EnemyState _nextState, bool _isChangeSameState = false)
    {
        if (!isInit) return;
        int hash;
        if (currentState == _nextState)
        {
            if (_isChangeSameState)
            {
                stateMachine.ChangeState(states[_nextState]);
                if (animationHash.TryGetValue(_nextState, out hash))
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

    protected override void Update()
    {
        base.Update();
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

    public void FindAttackTarget()
    {
        attackTarget = null;
        for (int i = 0; i < Managers.Object.Rangers.Count; i++)
        {
            if (attackTarget == null)
            {
                if (Managers.Object.Rangers[i].currentState != RangerState.Die)
                    attackTarget = Managers.Object.Rangers[i];
            }
            else
                break;
        }

        if (attackTarget != null)
        {
            for (int i = 0; i < Managers.Object.Rangers.Count; i++)
            {
                if (Managers.Object.Rangers[i].currentState != RangerState.Die)
                {
                    if (Vector2.Distance(transform.position, attackTarget.transform.position) > Vector2.Distance(transform.position, Managers.Object.Rangers[i].transform.position))
                        attackTarget = Managers.Object.Rangers[i];
                }
            }
        }
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
    }

    public void Follow()
    {
        enemy.Follow();
    }

    public void SetAttackTarget(RangerController _attackTarget)
    {
        attackTarget = _attackTarget;
    }

    public override void Die()
    {
        Stop();
        ReleseHPbar();
        Managers.Event.InvokeVoidEvent(VoidEventType.OnEnemyDead);
        StopAllCoroutines();
    }

    public override void CheckDie()
    {
        if (status.CurrentHP == 0)
            ChangeState(EnemyState.Die);
    }
}
public class EnemyStatus : ControllerStatus
{
    public EnemyStatus(BaseController _controller,EnemyControllerData _data)
    {
        controller = _controller;
        //���ݷ�
        defaultAttackForce = _data.attackForce;
        plustAttackForce = 0;

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
